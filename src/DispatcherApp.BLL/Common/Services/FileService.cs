using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.API.Controllers;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Common.Interfaces.Repository;
using DispatcherApp.BLL.Files.Commands.UpdateFile;
using DispatcherApp.BLL.Model;
using DispatcherApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FileMetadata = DispatcherApp.Models.Entities.File;

namespace DispatcherApp.BLL.Common.Services;
internal class FileService(
    IFileStorageService fileStorageService,
    IFileRepository fileRepository,
    IUserContextService userContextService
    ) : IFileService
{
    private readonly IFileStorageService _fileStorageService = fileStorageService;
    private readonly IFileRepository _fileRepository = fileRepository;
    private readonly IUserContextService _userContextService = userContextService;

    public async Task DeleteFileAsync(int id)
    {

        var fileMeta = await GetFileMetadataAsync(id);
        await _fileStorageService.RemoveFileAsync(fileMeta.StoragePath);

        _fileRepository.Remove(fileMeta);
        await _fileRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> DeleteMutipleFilesAsync(IEnumerable<int> ids, CancellationToken ct)
    {
        var metadatas = await _fileRepository.GetByIdsAsync(ids);

        foreach(var file in metadatas)
        {
            await _fileStorageService.RemoveFileAsync(file.StoragePath);
        }
        _fileRepository.RemoveRange(metadatas);
        await _fileRepository.SaveChangesAsync(ct);

        return metadatas.Select(f => f.OriginalFileName);
    }

    public async Task<FileContentResult> GetFileAsync(int id)
    {
        var fileMeta = await GetFileMetadataAsync(id);
        var fileStream = await _fileStorageService.LoadFileAsync(fileMeta.StoragePath);
        return new FileContentResult(fileStream, fileMeta.ContentType) { 
            FileDownloadName = fileMeta.OriginalFileName,
            LastModified = fileMeta.UploadedAt
        };
    }

    public async Task<FileMetadata> GetFileMetadataAsync(int id)
    {
        var file = await _fileRepository.GetByIdAsync(id);
        Guard.Against.NotFound(id, file);
        return file;
    }

    public async Task<IEnumerable<FileMetadata>> GetFilesMetadataAsync()
    {
        var files = await _fileRepository.GetAllAsync();
        return files;
    }

    public async Task<FileUploadResponse> SaveFileAsync(
        FileUploadRequest fur,
        CancellationToken cancellationToken)
    {
        var fileResult = await _fileStorageService.SaveFileStreamAsync(
            fur.FileStream,
            fur.ContentType,
            fur.Extension,
            fur.OriginalFileName
            );
        Guard.Against.Null(fileResult, nameof(fileResult));
        
        var file = _fileRepository.AddAsync(new FileMetadata
        {
            FileName = fileResult.FileName,
            OriginalFileName = fur.OriginalFileName,
            ContentType = fileResult.ContentType,
            FileSize = fur.FileStream.Length,
            StoragePath = fileResult.StoragePath,
            Description = fur.Description ?? "",
            UploadedByUserId = _userContextService.UserId,
            UploadedAt = DateTime.UtcNow
        });
        await _fileRepository.SaveChangesAsync(cancellationToken);

        return new FileUploadResponse
        {
            Id = file.Id,
        };

    }

}

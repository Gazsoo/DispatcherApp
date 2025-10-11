using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.API.Controllers;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Files.Commands.UpdateFile;
using DispatcherApp.BLL.Model;
using DispatcherApp.DAL.Data;
using DispatcherApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FileMetadata = DispatcherApp.Models.Entities.File;

namespace DispatcherApp.BLL.Common.Services;
internal class FileService(
    IFileStorageService fileStorageService,
    AppDbContext db,
    IUserContextService userContextService
    ) : IFileService
{
    private readonly IFileStorageService _fileStorageService = fileStorageService;
    private readonly AppDbContext _db = db;
    private readonly IUserContextService _userContextService = userContextService;

    public async Task DeleteFileAsync(int id)
    {

        var fileMeta = await GetFileMetadataAsync(id);
        await _fileStorageService.RemoveFileAsync(fileMeta.StoragePath);

        _db.Files.Remove(fileMeta);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> DeleteMutipleFilesAsync(IEnumerable<int> ids, CancellationToken ct)
    {
        var metadatas = await GetDbFileMetadatasByIds(ids).ToListAsync(ct);

        foreach(var file in metadatas)
        {
            await _fileStorageService.RemoveFileAsync(file.StoragePath);
        }
        _db.Files.RemoveRange(metadatas);
        await _db.SaveChangesAsync(ct);

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
        var file = await _db.Files
            .AsNoTracking()
            .SingleOrDefaultAsync(f => f.Id == id);
        Guard.Against.NotFound(id, file);
        return file;
    }

    public async Task<IEnumerable<FileMetadata>> GetFilesMetadataAsync()
    {
        var files = await _db.Files
            .AsNoTracking().
            ToListAsync();
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
        
        var uploadResult = _db.Files.Add(new FileMetadata
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
        await _db.SaveChangesAsync(cancellationToken);

        return new FileUploadResponse
        {
            Id = uploadResult.Entity.Id,
        };

    }
    private IQueryable<FileMetadata> GetDbFileMetadatasByIds(IEnumerable<int> ids)
    {
        var files = _db.Files
            .Where(f => ids.Contains(f.Id))
            .AsNoTracking();
        return files;
    }

}

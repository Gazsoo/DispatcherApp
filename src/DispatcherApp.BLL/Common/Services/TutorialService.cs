using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Configurations;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Common.Interfaces.Repository;
using DispatcherApp.BLL.Model;
using DispatcherApp.Models.DTOs.Tutorial;
using DispatcherApp.Models.Entities;
using DispatcherApp.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DispatcherApp.BLL.Common.Services;
public class TutorialService : ITutorialService
{
    private readonly ITutorialRepository _tutorialRepository;
    private readonly FileStorageSettings _fileStorageSettings;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;

    public TutorialService(ITutorialRepository tutorialRepository,
        IOptions<FileStorageSettings> fileStorageSettings,
        IMapper mapper,
        IFileStorageService fileStorageService)
    {
        _fileStorageSettings = fileStorageSettings.Value;
        _fileStorageService = fileStorageService;
        _mapper = mapper;
        _tutorialRepository = tutorialRepository;
    }
    public async Task<int> AddTutorialFileAsync(IFormFile file, int tutorialId)
    {
        var tutorial = await _tutorialRepository.GetByIdAsync(tutorialId);
        Guard.Against.NotFound(tutorialId, tutorial);

        var maxFileSize = Guard.Against.Null(_fileStorageSettings.MaxFileSize, nameof(_fileStorageSettings.MaxFileSize));
        Guard.Against.OutOfRange(file.Length, nameof(file.Length), 1, maxFileSize, null, () => new ValidationException("File size limit"));

        var saveResult = await _fileStorageService.SaveFileAsync(file, GetTutorialRelativePath(tutorialId));
        Guard.Against.NullOrEmpty(saveResult.StoragePath, nameof(saveResult.StoragePath));

        var tutorialFile = new Models.Entities.File
        {
            FileName = saveResult.FileName,
            OriginalFileName = file.FileName,
            ContentType = file.ContentType ?? "application/octet-stream",
            FileSize = file.Length,
            StoragePath = saveResult.StoragePath,
            UploadedAt = DateTime.UtcNow,
            Tutorials = new List<Tutorial> { tutorial }
        };

        //_tutorialRepository.Files.Add(tutorialFile);
        await _tutorialRepository.SaveChangesAsync();
        return tutorialFile.Id;
    }
    private string GetTutorialRelativePath(int tutorialId) =>
        Path.Combine("tutorials", tutorialId.ToString());

    public async Task<FileResult> GetTutorialFileAsync(int fileId, int tutorialId)
    {
        var tutorial = await _tutorialRepository.GetByIdAsync(tutorialId);
        Guard.Against.NotFound(tutorialId, tutorial);

        //var file = _tutorialRepository.Files.FirstOrDefault(t => t.Id == fileId);
        //Guard.Against.NotFound(fileId, file);

        //var content = await _fileStorageService.LoadFileAsync(
        //    Path.Combine(GetTutorialRelativePath(tutorialId), file.FileName)
        //    );
        //Guard.Against.NullOrEmpty(content, nameof(content), null, () => new ValidationException("File content is empty"));

        return new FileResult
        {
            //ContentType = file.ContentType,
            //FileContent = content,
            //FileName = file.OriginalFileName
        };
    }

    public async Task<TutorialResponse> GetTutorialAsync(int tutorialId)
    {
        var tutorial = await _tutorialRepository.GetByIdAsync(tutorialId, includeFiles: true);
        return _mapper.Map<TutorialResponse>(tutorial);
    }

    public async Task<List<TutorialResponse>> GetTutorialListAsync()
    {
        var tutorials = await _tutorialRepository.GetAllAsync(includeFiles: true);

        return _mapper.Map<List<TutorialResponse>>(tutorials);
    }

    public Task<CreateTutorialResponse> CreateTutorial(CreateTutorialRequest request)
    {
        throw new NotImplementedException();
    }
}

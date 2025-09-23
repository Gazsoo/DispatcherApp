using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Configurations;
using DispatcherApp.BLL.Interfaces;
using DispatcherApp.DAL.Data;
using DispatcherApp.Models.Entities;
using DispatcherApp.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DispatcherApp.BLL.Services;
public class TutorialService : ITutorialService
{
    private readonly AppDbContext _context;
    private readonly FileStorageSettings _fileStorageSettings;
    private readonly IFileStorageService _fileStorageService;

    public TutorialService(AppDbContext context,
        IOptions<FileStorageSettings> fileStorageSettings,
        IFileStorageService fileStorageService)
    {
        _fileStorageSettings = fileStorageSettings.Value;
        _fileStorageService = fileStorageService;
        _context = context;
    }
    public async Task<int> AddTutorialFileAsync(IFormFile file, int tutorialId)
    {
        var tutorial = _context.Tutorials.FirstOrDefault(t => t.Id == tutorialId);
        Guard.Against.NotFound(tutorialId, tutorial);

        var maxFileSize = Guard.Against.Null(_fileStorageSettings.MaxFileSize, nameof(_fileStorageSettings.MaxFileSize));
        Guard.Against.OutOfRange(file.Length, nameof(file.Length), 1, maxFileSize, null, () => new ValidationException("File size limit"));

        var soragePaht = await _fileStorageService.SaveFileAsync(file, tutorialId);
        Guard.Against.NullOrEmpty(soragePaht, nameof(soragePaht));

        var tutorialFile = new Models.Entities.File
        {
            TutorialId = tutorialId,
            FileName = file.FileName,
            OriginalFileName = file.FileName,
            ContentType = file.ContentType ?? "application/octet-stream",
            FileSize = file.Length,
            StoragePath = soragePaht,
            UploadedAt = DateTime.UtcNow
        };

        _context.Files.Add(tutorialFile);
        await _context.SaveChangesAsync();
        return tutorialFile.Id;
    }
}

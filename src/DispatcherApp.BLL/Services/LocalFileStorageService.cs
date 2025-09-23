using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Configurations;
using DispatcherApp.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DispatcherApp.BLL.Services;
public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;
    public LocalFileStorageService(IOptions<FileStorageSettings> options)
    {
        _basePath = Guard.Against.NullOrEmpty(options.Value.BasePath);
    }

    public async Task<string> SaveFileAsync(IFormFile file, int tutorialId)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var relativePath = Path.Combine("tutorials", tutorialId.ToString(), fileName);
        var fullPath = Path.Combine(_basePath, relativePath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ??
            throw new ArgumentException("no file path"));

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return relativePath; // Store this in TutorialFile.StoragePath
    }
}

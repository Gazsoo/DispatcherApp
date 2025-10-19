using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Model;
using DispatcherApp.Common.Abstractions.Storage;
using DispatcherApp.Common.Entities;
using DispatcherApp.DAL.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using File = System.IO.File;

namespace DispatcherApp.DAL.Services;
public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;
    public LocalFileStorageService(IOptions<FileStorageSettings> options)
    {
        _basePath = Guard.Against.NullOrEmpty(options.Value.BasePath);
    }

    public async Task<byte[]> LoadFileAsync(string relativePath)
    {
        var fullPath = GetFullPath(relativePath);
        var content = await System.IO.File.ReadAllBytesAsync(fullPath);
        return content;
    }

    private string GetFullPath(string relativePath, string fileName = "")
    {
        return Path.Combine(_basePath, relativePath, fileName);
    }

    public async Task<SaveFileResult> SaveFileStreamAsync(
        Stream fileStream,
        string contentType,
        string extention,
        string name)
    {
        var fileName = $"{Guid.NewGuid()}{extention}";
        var relativePathWith = Path.Combine("files", fileName);
        var fullPath = Path.Combine(_basePath, relativePathWith);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ??
            throw new ArgumentException("no file path"));

        using var stream = new FileStream(fullPath, FileMode.Create);
        await fileStream.CopyToAsync(stream);

        return new SaveFileResult
        {
            ContentType = contentType ?? "application/octet-stream",
            FileName = fileName,
            StoragePath = Path.Combine(relativePathWith)
        };
    }

    public async Task<SaveFileResult> SaveFileAsync(IFormFile file, string relativePathWithoutFileName)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var fullPath = GetFullPath(relativePathWithoutFileName, fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ??
            throw new ArgumentException("no file path"));

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return new SaveFileResult
        {
            ContentType = file.ContentType ?? "application/octet-stream",
            FileName = fileName,
            StoragePath = Path.Combine(relativePathWithoutFileName, fileName)
        };
    }

    public async Task RemoveFileAsync(string relativePath)
    {
        var fullPath = GetFullPath(relativePath);
        try
        {
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
        catch (IOException ex)
        {
            // Log warning or retry logic here
            Console.WriteLine($"Error deleting file: {ex.Message}");
        }
        await Task.CompletedTask;
    }
}

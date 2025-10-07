using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Configurations;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Model;
using DispatcherApp.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DispatcherApp.BLL.Common.Services;
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
}

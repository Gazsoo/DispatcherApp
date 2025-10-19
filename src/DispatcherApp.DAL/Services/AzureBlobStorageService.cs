using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using DispatcherApp.Common.Abstractions.Storage;
using DispatcherApp.Common.Exceptions;
using DispatcherApp.DAL.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DispatcherApp.DAL.Services;
internal class AzureBlobStorageService(
    BlobServiceClient blobClient,
    TokenCredential credential,
    IOptions<FileStorageSettings> settings
    ) : IFileStorageService
{
    private readonly BlobServiceClient _blobClient = blobClient;
    private readonly FileStorageSettings _settings = settings.Value;
    private readonly TokenCredential _credential = credential;

    public async Task<byte[]> LoadFileAsync(string storagePath)
    {
        if (Uri.IsWellFormedUriString(storagePath, UriKind.Absolute))
        {
            var blobClient = new BlobClient(new Uri(storagePath), _credential);
            var response = await blobClient.DownloadContentAsync();
            return response.Value.Content.ToArray();
        } else
        {
            throw new NotFoundException("Blob", storagePath);
        }
    }

    public async Task RemoveFileAsync(string storagePath)
    {
        if (Uri.IsWellFormedUriString(storagePath, UriKind.Absolute))
        {
            var blobClient = new BlobClient(new Uri(storagePath), _credential);
            var response = await blobClient.DeleteIfExistsAsync();
            return;
        }
        else
        {
            throw new NotFoundException("Blob", storagePath);
        }
    }

    public Task<SaveFileResult> SaveFileAsync(IFormFile file, string relativePathWithoutFileName)
    {
        throw new NotImplementedException();
    }

    public async Task<SaveFileResult> SaveFileStreamAsync(Stream fileStream, string contentType, string extention, string name)
    {
        var container = _blobClient.GetBlobContainerClient(_settings.ContainerName);
        var result = await container.UploadBlobAsync(name, fileStream);
        var blobUri = container.GetBlobClient(name).Uri.ToString();
        return new SaveFileResult
        {
            ContentType = contentType ?? "application/octet-stream",
            FileName = name,
            StoragePath = blobUri
        };
    }
}

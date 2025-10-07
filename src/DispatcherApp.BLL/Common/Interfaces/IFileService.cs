using DispatcherApp.BLL.Model;
using Microsoft.AspNetCore.Http;

namespace DispatcherApp.API.Controllers;

public interface IFileService
{
    Task<List<FileMetadataResponse>> GetFilesMetadataAsync();
    Task<FileResult> GetFileAsync(int id);
    Task<FileUploadResponse> SaveFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken);
    Task DeleteFileAsync(int id);
    Task DeleteMutipleFilesAsync(List<int> ids);
}

using DispatcherApp.BLL.Model;
using Microsoft.AspNetCore.Http;

namespace DispatcherApp.BLL.Common.Interfaces;
public interface IFileStorageService
{
    Task<SaveFileResult> SaveFileAsync(IFormFile file, string relativePathWithoutFileName);
    Task<SaveFileResult> SaveFileStreamAsync(
        Stream fileStream,
        string contentType,
        string extention,
        string name);
    Task<byte[]>  LoadFileAsync(string relativePath);
    Task RemoveFileAsync(string relativePath);
}

using DispatcherApp.BLL.Model;
using Microsoft.AspNetCore.Http;

namespace DispatcherApp.BLL.Interfaces;
public interface IFileStorageService
{
    Task<SaveFileResult> SaveFileAsync(IFormFile file, string relativePathWithoutFileName);
    Task<byte[]>  LoadFileAsync(string relativePath);
}

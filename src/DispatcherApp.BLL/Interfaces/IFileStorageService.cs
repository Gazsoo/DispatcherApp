using Microsoft.AspNetCore.Http;

namespace DispatcherApp.BLL.Interfaces;
public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, int tutorialId);
}
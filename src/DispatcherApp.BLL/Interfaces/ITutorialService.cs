using DispatcherApp.BLL.Model;
using DispatcherApp.Models.DTOs.Tutorial;
using Microsoft.AspNetCore.Http;

namespace DispatcherApp.BLL.Interfaces;
public interface ITutorialService
{
    Task<int> AddTutorialFileAsync(IFormFile file, int tutorialId);
    Task<FileResult> GetTutorialFileAsync(int fileId, int tutorialId);
    Task<TutorialResponse> GetTutorialAsync(int tutorialId);
    Task<List<TutorialResponse>> GetTutorialListAsync();
}

using Microsoft.AspNetCore.Http;

namespace DispatcherApp.BLL.Interfaces;
public interface ITutorialService
{
    Task<int> AddTutorialFileAsync(IFormFile file, int tutorialId);
}

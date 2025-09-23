using DispatcherApp.BLL.Interfaces;
using DispatcherApp.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DispatcherApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TutorialController (ITutorialService tutorialService): ControllerBase
{
    private readonly ITutorialService _tutorialService = tutorialService;

    [HttpPost("{tutorialId}/files")]
    public async Task<IActionResult> UploadFile(int tutorialId, IFormFile file)
    {
        var result = await _tutorialService.AddTutorialFileAsync(file, tutorialId);
        return Ok(result);
    }
}

using DispatcherApp.BLL.Interfaces;
using DispatcherApp.BLL.Services;
using DispatcherApp.Models.DTOs.Tutorial;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DispatcherApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TutorialController (ITutorialService tutorialService): ControllerBase
{
    private readonly ITutorialService _tutorialService = tutorialService;

    [HttpPost("{tutorialId}/files")]
    public async Task<ActionResult<int>> UploadFile(int tutorialId, IFormFile file)
    {
        var result = await _tutorialService.AddTutorialFileAsync(file, tutorialId);
        return Ok(result);
    }

    [HttpGet("{tutorialId}/files/{fileId}")]
    public async Task<FileContentResult> GetFile(int tutorialId, int fileId)
    {
        var result = await _tutorialService.GetTutorialFileAsync(fileId, tutorialId);
        return File(result.FileContent, result.ContentType);
    }

    [HttpGet("{tutorialId}")]
    public async Task<ActionResult<TutorialResponse>> GetTutorial(int tutorialId)
    {
        var result = await _tutorialService.GetTutorialAsync(tutorialId);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<TutorialResponse>>> GetTutorial()
    {
        var result = await _tutorialService.GetTutorialListAsync();
        return Ok(result);
    }

}

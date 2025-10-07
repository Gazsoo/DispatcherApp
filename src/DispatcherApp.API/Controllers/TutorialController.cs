using DispatcherApp.BLL.Common.Interfaces;
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
    public async Task<ActionResult<int>> UploadTutorialFile(int tutorialId, IFormFile file)
    {
        var result = await _tutorialService.AddTutorialFileAsync(file, tutorialId);
        return Ok(result);
    }

    [HttpGet("{tutorialId}/files/{fileId}")]
    public async Task<FileContentResult> GetTutorialFile(int tutorialId, int fileId)
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
    public async Task<ActionResult<List<TutorialResponse>>> GetTutorials()
    {
        var result = await _tutorialService.GetTutorialListAsync();
        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<CreateTutorialResponse>> CreateTutorial([FromBody]CreateTutorialRequest request)
    {
        var result = await _tutorialService.CreateTutorial(request);
        return Ok(result);
    }

    //[HttpPost]
    //public async Task<ActionResult<CreateTutorialResponse>> CreateTutorial([FromBody] CreateTutorialRequest request)
    //{
    //    var result = await _tutorialService.CreateTutorial(request);
    //    return Ok(result);
    //}
}

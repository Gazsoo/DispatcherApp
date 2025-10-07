using DispatcherApp.BLL.Files.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DispatcherApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilesController (
    IMediator mediator
    ) : ControllerBase
{
    private readonly IFileService _fileService = fileService;
    // GET: api/<FilesController>
    [HttpGet]
    public async Task<ActionResult<List<FileMetadataResponse>>> GetFilesAsync()
    {
        var result = await _fileService.GetFilesMetadataAsync();
        return Ok(result);
    }

    // GET api/<FilesController>/5
    [HttpGet("{fileId}/download")]
    public async Task<FileContentResult> Get(int fileId)
    {
        var result = await _fileService.GetFileAsync(fileId);
        return File(result.FileContent, result.ContentType, result.FileName);
    }

    // POST api/<FilesController>
    [HttpPost]
    public async Task<ActionResult<FileUploadResponse>> PostFileAsync([FromForm] UploadFileCommand uploadCommand)
    {
        var result = await mediator.Send(uploadCommand);
        return Ok(result);
    }


    // DELETE api/<FilesController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFile(int fileId)
    {
        await _fileService.DeleteFileAsync(fileId);
        return Ok();
    }
    [HttpDelete("/files")]
    public async Task<ActionResult> DeleteMutipleFile([FromBody] List<int> fileIds)
    {
        await _fileService.DeleteMutipleFilesAsync(fileIds);
        return Ok();
    }
}

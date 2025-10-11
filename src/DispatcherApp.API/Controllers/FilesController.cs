using System.Runtime.InteropServices;
using DispatcherApp.BLL.Files.Commands.DeleteFile;
using DispatcherApp.BLL.Files.Commands.DeleteFiles;
using DispatcherApp.BLL.Files.Commands.UpdateFile;
using DispatcherApp.BLL.Files.Queries.DownloadFile;
using DispatcherApp.BLL.Files.Queries.GetFile;
using DispatcherApp.BLL.Files.Queries.GetFiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DispatcherApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilesController (IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FileMetadataResponse>>> GetFilesAsync()
    {
        var result = await mediator.Send(new GetFilesQuery());
        return Ok(result);
    }

    [HttpGet("{fileId}/download")]
    public async Task<FileContentResult> DownloadFile(int fileId)
    {
        var result = await mediator.Send(new DownloadFileQuery(fileId));
        return result;
    }

    [HttpGet("{fileId}")]
    public async Task<ActionResult<FileMetadataResponse>> GetFileMetadata(int fileId)
    {
        var result = await mediator.Send(new GetFileQuery(fileId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<FileUploadResponse>> PostFileAsync([FromForm] UploadFileCommand uploadCommand)
    {
        var result = await mediator.Send(uploadCommand);
        return Ok(result);
    }

    [HttpDelete("{fileId}")]
    public async Task<ActionResult> DeleteFile(int fileId)
    {
        var result = await mediator.Send(new DeleteFileCommand(fileId));
        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteMutipleFiles([FromBody] DeleteFilesCommand deleteCommand)
    {
        var result = await mediator.Send(deleteCommand);
        return Ok(result);
    }
}

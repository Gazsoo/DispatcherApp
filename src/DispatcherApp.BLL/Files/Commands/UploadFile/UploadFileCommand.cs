using DispatcherApp.Common.DTOs.Files;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DispatcherApp.BLL.Files.Commands.UpdateFile;
public class UploadFileCommand :  IRequest<FileUploadResponse>
{
    public required IFormFile File { get; set; }
    public string? Description { get; set; }
}

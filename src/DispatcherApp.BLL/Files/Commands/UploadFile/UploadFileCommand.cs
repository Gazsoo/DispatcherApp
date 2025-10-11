using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DispatcherApp.BLL.Files.Commands.UpdateFile;
public class UploadFileCommand :  IRequest<FileUploadResponse>
{
    public required IFormFile File { get; set; }
    public string? Description { get; set; }
}

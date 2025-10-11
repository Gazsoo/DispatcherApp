using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.API.Controllers;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Common.Services;
using DispatcherApp.DAL.Data;
using MediatR;

namespace DispatcherApp.BLL.Files.Commands.UpdateFile;
public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, FileUploadResponse>
{
    public readonly IFileService _fileService;
    public readonly AppDbContext _context;
    public readonly IUserContextService _userContextService;

    public UploadFileCommandHandler(
        IFileService fileService,
        AppDbContext context,
        IUserContextService userContextService
        )
    {
        _fileService = fileService;
        _context = context;
        _userContextService = userContextService;
    }

    public async Task<FileUploadResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var extention = Path.GetExtension(request.File.FileName);
        var fileEntity = await _fileService
            .SaveFileAsync( 
                new FileUploadRequest(
                    request.File.OpenReadStream(),
                    request.File.FileName,
                    request.File.ContentType,
                    extention,
                    request.Description
                ), 
                cancellationToken
                );

        return new FileUploadResponse
        {
            Id = fileEntity.Id,
        };
    }
}

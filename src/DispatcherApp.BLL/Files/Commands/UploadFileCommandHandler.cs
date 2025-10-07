using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.API.Controllers;
using DispatcherApp.BLL.Common.Services;
using DispatcherApp.DAL.Data;
using MediatR;

namespace DispatcherApp.BLL.Files.Commands;
public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, FileUploadResponse>
{
    public readonly IFileService _fileService;
    public readonly AppDbContext _context;
    public readonly UserContextService _userContextService;

    public UploadFileCommandHandler(
        IFileService fileService,
        AppDbContext context,
        UserContextService userContextService
        )
    {
        _fileService = fileService;
        _context = context;
        _userContextService = userContextService;
    }

    public async Task<FileUploadResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
        var contentType = request.File.ContentType ?? "application/octet-stream";

        var saveResponse = await _fileService.SaveFileAsync(
            request.File.OpenReadStream(),
            fileName,
            contentType,
            cancellationToken
            );

        var fileEntity = new Models.Entities.File
        {
            FileName = fileName,
            OriginalFileName = request.File.FileName,
            UploadedByUserId = _userContextService.UserId,
            ContentType = contentType,
            FileSize = request.File.Length,
            StoragePath = "",
            UploadedAt = DateTime.UtcNow
        };
        _context.Files.Add(fileEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return new FileUploadResponse
        {
            Id = fileEntity.Id,
        };
    }
}

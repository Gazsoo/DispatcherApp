using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Files;
using DispatcherApp.Common.Exceptions;
using MediatR;

namespace DispatcherApp.BLL.Files.Commands.UploadFile;
public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, FileUploadResponse>
{
    private readonly IFileService _fileService;

    public UploadFileCommandHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<FileUploadResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        using var stream = request.File.OpenReadStream();
        var extension = Path.GetExtension(request.File.FileName);
        Guard.Against.NullOrEmpty(extension, nameof(extension), null, () => new ValidationException("File extension is missing"));

        var fileEntity = await _fileService.SaveFileAsync(
            new FileUploadData(
                stream,
                request.File.FileName,
                request.File.ContentType,
                extension,
                request.Description),
            cancellationToken);

        return new FileUploadResponse
        {
            Id = fileEntity.Id,
        };
    }
}

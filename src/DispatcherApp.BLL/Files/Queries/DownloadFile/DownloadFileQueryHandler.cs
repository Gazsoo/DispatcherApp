using DispatcherApp.BLL.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DispatcherApp.BLL.Files.Queries.DownloadFile;
internal sealed class DownloadFileQueryHandler(
    IFileService fileService
    ) : IRequestHandler<DownloadFileQuery, FileContentResult>
{
    private readonly IFileService _fileService = fileService;
    public async Task<FileContentResult> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
    {
        var fileMeta = await _fileService.GetFileMetadataAsync(request.Id);
        return await _fileService.GetFileAsync(request.Id);
    }
}

using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Files.Commands.DeleteFile;
using MediatR;

namespace DispatcherApp.BLL.Files.Commands.DeleteFiles;
internal sealed class DeleteFilesCommandHandler(
    IFileService fileService
    ) : IRequestHandler<DeleteFilesCommand, DeleteFilesCommandResponse>
{
    private readonly IFileService _fileService = fileService;

    public async Task<DeleteFilesCommandResponse> Handle(DeleteFilesCommand request, CancellationToken cancellationToken)
    {
        var result = await _fileService.DeleteMutipleFilesAsync(request.Ids, cancellationToken);
        return new DeleteFilesCommandResponse
        {
            DeletedFileNames = result
        };
    }
}

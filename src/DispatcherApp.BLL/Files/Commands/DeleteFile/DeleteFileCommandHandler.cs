using DispatcherApp.BLL.Common.Interfaces;
using MediatR;

namespace DispatcherApp.BLL.Files.Commands.DeleteFile;
internal sealed class DeleteFileCommandHandler(
    IFileService fileService
    ) : IRequestHandler<DeleteFileCommand, DeleteFileCommandResponse>
{
    private readonly IFileService _fileService = fileService;
    public async Task<DeleteFileCommandResponse> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        await _fileService.DeleteFileAsync(request.Id);
        return new DeleteFileCommandResponse
        {
            Success = true,
        };
    }
}

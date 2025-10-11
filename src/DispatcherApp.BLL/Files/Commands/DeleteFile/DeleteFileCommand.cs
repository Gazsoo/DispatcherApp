using MediatR;

namespace DispatcherApp.BLL.Files.Commands.DeleteFile;
// Include properties to be used as input for the command
public record DeleteFileCommand(int Id) : IRequest<DeleteFileCommandResponse>
{
};

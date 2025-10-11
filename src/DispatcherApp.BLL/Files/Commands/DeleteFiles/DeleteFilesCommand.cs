using MediatR;

namespace DispatcherApp.BLL.Files.Commands.DeleteFiles;
// Include properties to be used as input for the command
public record DeleteFilesCommand(IEnumerable<int> Ids) : IRequest<DeleteFilesCommandResponse>;

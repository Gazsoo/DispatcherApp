using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands.LeaveSession;
// Include properties to be used as input for the command
public record LeaveSessionCommand(string SessionId) : IRequest<Unit>;

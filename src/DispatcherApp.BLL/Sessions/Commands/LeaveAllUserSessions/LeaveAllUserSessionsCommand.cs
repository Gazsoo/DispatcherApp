using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands.LeaveAllUserSessions;
// Include properties to be used as input for the command
public record LeaveAllUserSessionsCommand() : IRequest<Unit>;

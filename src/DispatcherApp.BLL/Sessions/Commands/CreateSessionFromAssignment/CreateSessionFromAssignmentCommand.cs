using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands.CreateSessionFromAssignment;
// Include properties to be used as input for the command
public record CreateSessionFromAssignmentCommand(int AssignmentId) : IRequest<SessionResponse>;

using MediatR;

namespace DispatcherApp.BLL.User.Commands.DeleteUser;
// Include properties to be used as input for the command
public record DeleteUserCommand(string UserId) : IRequest<Unit>;

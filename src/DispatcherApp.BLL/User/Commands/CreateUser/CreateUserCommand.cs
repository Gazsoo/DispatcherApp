using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.User.Commands.CreateUser;
// Include properties to be used as input for the command
public record CreateUserCommand(CreateUserRequest Request) : IRequest<UserInfoResponse>;

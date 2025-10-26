using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.User.Commands.UpdateUser;
// Include properties to be used as input for the command
public record UpdateUserCommand(UserInfoUpdateRequest Request) : IRequest<UserInfoResponse>;

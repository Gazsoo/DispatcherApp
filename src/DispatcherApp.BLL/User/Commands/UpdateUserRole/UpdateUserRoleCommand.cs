using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.User.Commands.UpdateUserRole;
// Include properties to be used as input for the command
public record UpdateUserRoleCommand(string UserId, string Role) : IRequest<UserInfoResponse>;

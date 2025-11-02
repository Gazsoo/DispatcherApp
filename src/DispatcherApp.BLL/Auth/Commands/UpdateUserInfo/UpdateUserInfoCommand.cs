using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.UpdateUserInfo;

public sealed record UpdateUserInfoCommand(string UserId, UserInfoResponse UserInfo) : IRequest<bool>;

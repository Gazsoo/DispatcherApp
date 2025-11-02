using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.Auth.Queries.GetUserInfo;

public sealed record GetUserInfoQuery(string UserId) : IRequest<UserInfoResponse?>;

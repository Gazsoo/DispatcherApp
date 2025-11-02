using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DispatcherApp.BLL.User.Queries.GetUser;
internal sealed class GetUserQueryHandler(
        IUserService _userService
    ) : IRequestHandler<GetUserQuery, UserInfoResponse>
{
    public async Task<UserInfoResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetByIdAsync(request.Id, cancellationToken);
    }
}

using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DispatcherApp.BLL.User.Commands.UpdateUserRole;
internal sealed class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, UserInfoResponse>
{
    private readonly IUserService _userService;

    public UpdateUserRoleCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserInfoResponse> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.UpdateRoleAsync(request.UserId, request.Role, cancellationToken);
        Guard.Against.Null(user, nameof(user));

        return user;
    }
}

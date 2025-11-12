using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DispatcherApp.BLL.User.Queries.GetAllUsers;
internal sealed class GetAllUsersQueryHandler(
    IUserProfileService userProfileService
    ) : IRequestHandler<GetAllUsersQuery, GetAllUsersResponse>
{
    private readonly IUserProfileService _userProfileService = userProfileService;
    public async Task<GetAllUsersResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userProfileService.GetAllAsync(cancellationToken);
        return new GetAllUsersResponse(users);
    }
}

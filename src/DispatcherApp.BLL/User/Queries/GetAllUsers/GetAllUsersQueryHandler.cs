using AutoMapper;
using DispatcherApp.Common.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DispatcherApp.BLL.User.Queries.GetAllUsers;
internal sealed class GetAllUsersQueryHandler(
    UserManager<IdentityUser> _userManager,
    IMapper _mapper
    ) : IRequestHandler<GetAllUsersQuery, GetAllUsersResponse>
{
    public async Task<GetAllUsersResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var userList = await _userManager.Users.ToListAsync(cancellationToken);
        var mappedUsers = _mapper.Map<List<UserInfoResponse>>(userList);
        return new GetAllUsersResponse(Users : mappedUsers);
    }
}

using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.User.Queries.GetProfile;
internal sealed class GetProfileQueryHandler (
    IUserContextService userContextService,
    IUserService userService
    ) : IRequestHandler<GetProfileQuery, UserInfoResponse>
{
    private readonly IUserContextService _userContextService = userContextService;
    private readonly IUserService _userService = userService;
    public async Task<UserInfoResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var me = _userContextService.GetCurrentUser();
        Guard.Against.Null(me, nameof(me));
        Guard.Against.Null(me.UserId, nameof(me.UserId));
        return await _userService.GetByIdAsync(me.UserId, cancellationToken);
    }
}

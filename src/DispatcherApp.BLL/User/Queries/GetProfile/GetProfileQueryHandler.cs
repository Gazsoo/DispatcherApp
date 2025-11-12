using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.User.Queries.GetProfile;
internal sealed class GetProfileQueryHandler (
    IUserContextService userContextService,
    IUserProfileService userProfileService
    ) : IRequestHandler<GetProfileQuery, UserInfoResponse>
{
    private readonly IUserContextService _userContextService = userContextService;
    private readonly IUserProfileService _userProfileService = userProfileService;
    public async Task<UserInfoResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var me = _userContextService.GetCurrentUser();
        Guard.Against.Null(me, nameof(me));
        Guard.Against.Null(me.UserId, nameof(me.UserId));
        return await _userProfileService.GetAsync(me.UserId, cancellationToken);
    }
}

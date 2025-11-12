using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.Auth.Queries.GetUserInfo;

internal sealed class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoResponse>
{
    private readonly IUserProfileService _userProfileService;

    public GetUserInfoQueryHandler(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    public async Task<UserInfoResponse> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        return await _userProfileService.GetAsync(request.UserId, cancellationToken);
    }
}

using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.User.Queries.GetUser;
internal sealed class GetUserQueryHandler(
        IUserProfileService userProfileService
    ) : IRequestHandler<GetUserQuery, UserInfoResponse>
{
    private readonly IUserProfileService _userProfileService = userProfileService;
    public async Task<UserInfoResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await _userProfileService.GetAsync(request.Id, cancellationToken);
    }
}

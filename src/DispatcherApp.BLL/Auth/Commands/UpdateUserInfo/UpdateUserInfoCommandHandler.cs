using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.User;
using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.UpdateUserInfo;

internal sealed class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand, UserInfoResponse>
{
    private readonly IUserProfileService _userProfileService;

    public UpdateUserInfoCommandHandler(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    public async Task<UserInfoResponse> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        return await _userProfileService.UpdateAsync(request.UserId, request.UserInfo, cancellationToken);
    }
}

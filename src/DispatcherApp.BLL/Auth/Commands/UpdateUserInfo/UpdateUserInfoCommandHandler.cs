using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.UpdateUserInfo;

internal sealed class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand, bool>
{
    private readonly IAuthenticationService _authenticationService;

    public UpdateUserInfoCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<bool> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationService.UpdateUserInfoAsync(request.UserId, request.UserInfo);
    }
}

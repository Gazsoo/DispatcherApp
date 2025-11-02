using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Auth;
using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.Refresh;

internal sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthResponse?>
{
    private readonly IAuthenticationService _authenticationService;

    public RefreshCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<AuthResponse?> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationService.RefreshTokenAsync(request.Request);
    }
}

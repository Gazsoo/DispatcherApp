using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Auth.Commands.Login;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Auth;
using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.Login;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse?>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<AuthResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationService.LoginAsync(request.Request);
    }
}

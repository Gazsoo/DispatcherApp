using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.ResendConfirmationEmail;

internal sealed class ResendConfirmationEmailCommandHandler : IRequestHandler<ResendConfirmationEmailCommand, bool>
{
    private readonly IAuthenticationService _authenticationService;

    public ResendConfirmationEmailCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<bool> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationService.ResendConfirmationEmailAsync(request.Email, request.ConfirmationBaseUrl);
    }
}

using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.ConfirmEmail;

internal sealed class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
{
    private readonly IAuthenticationService _authenticationService;

    public ConfirmEmailCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationService.ConfirmEmailAsync(request.UserId, request.Token);
    }
}

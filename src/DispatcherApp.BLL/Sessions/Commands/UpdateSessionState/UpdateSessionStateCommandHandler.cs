using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.Abstractions;
using DispatcherApp.Common.DTOs.Session;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace DispatcherApp.BLL.Sessions.Commands.UpdateSessionState;
internal sealed class UpdateSessionStateCommandHandler(
    ISessionService sessionService 
    ) : IRequestHandler<UpdateSessionStateCommand, SessionResponse>
{
    private readonly ISessionService _sessionService = sessionService;

    public async Task<SessionResponse> Handle(UpdateSessionStateCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionService.UpdateSessionStatusAsync(request.sessionId, request.dss, cancellationToken);

        await _sessionService.SendOutSessionsAcitvityAsync(cancellationToken);

        return session;

    }
}

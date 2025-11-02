using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Assignment;
using DispatcherApp.Common.DTOs.Session;
using MediatR;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace DispatcherApp.BLL.Sessions.Commands.CreateSession;

internal sealed class GetOrCreateSessionCommandHandler : IRequestHandler<JoinGetOrCreateSessionCommand, SessionResponse>
{
    private readonly ISessionService _sessionService;
    private readonly IUserService _userService;
    private readonly IUserContextService _userContextService;

    public GetOrCreateSessionCommandHandler(
        ISessionService sessionService,
        IUserService userService,
        IUserContextService userContextService
        )
    {
        _userService = userService;
        _userContextService = userContextService;
        _sessionService = sessionService;
    }

    public async Task<SessionResponse> Handle(JoinGetOrCreateSessionCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.UserId;
        Guard.Against.NullOrEmpty(userId, nameof(userId));
        var session = await _sessionService.JoinOrCreateAsync(request.SessionId, userId, cancellationToken);
        await _sessionService.SendOutSessionsAcitvityAsync(cancellationToken);

        return session;
    }
}

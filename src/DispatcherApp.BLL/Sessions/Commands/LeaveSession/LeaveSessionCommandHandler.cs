using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands.LeaveSession;
internal sealed class LeaveSessionCommandHandler (
    ISessionService sessionService,
    IUserContextService userContextService
    ) : IRequestHandler<LeaveSessionCommand, Unit>
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IUserContextService _userContextService = userContextService;
    public async Task<Unit> Handle(LeaveSessionCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.UserId;
        Guard.Against.NullOrEmpty(userId, nameof(userId));
        await _sessionService.LeaveSessionAsync(request.SessionId, userId, cancellationToken);
        return Unit.Value;
    }
}

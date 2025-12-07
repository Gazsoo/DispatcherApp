using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.Abstractions.Repository;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands.LeaveSession;
internal sealed class LeaveSessionCommandHandler (
    ISessionService sessionService,
    IUserContextService userContextService,
    ISessionRepository sessionRepository
    ) : IRequestHandler<LeaveSessionCommand, Unit>
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IUserContextService _userContextService = userContextService;
    private readonly ISessionRepository _sessionRepository = sessionRepository;
    public async Task<Unit> Handle(LeaveSessionCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.UserId;
        Guard.Against.NullOrEmpty(userId, nameof(userId));
        await _sessionService.LeaveSessionAsync(request.SessionId, userId, cancellationToken);

        if (request.LogFileId is not null)
            await _sessionRepository.AddLogFile(request.LogFileId.Value, request.SessionId, cancellationToken);

        await _sessionService.SendOutSessionsAcitvityAsync(cancellationToken);
        return Unit.Value;
    }
}

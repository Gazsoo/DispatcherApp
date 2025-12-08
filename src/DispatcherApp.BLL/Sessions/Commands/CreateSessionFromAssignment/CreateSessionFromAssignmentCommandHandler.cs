using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands.CreateSessionFromAssignment;
internal sealed class CreateSessionFromAssignmentCommandHandler : IRequestHandler<CreateSessionFromAssignmentCommand, SessionResponse>
{
    private readonly ISessionService _sessionService;
    private readonly IUserService _userService;
    private readonly IUserContextService _userContextService;

    public CreateSessionFromAssignmentCommandHandler(
        ISessionService sessionService,
        IUserService userService,
        IUserContextService userContextService
        )
    {
        _userService = userService;
        _userContextService = userContextService;
        _sessionService = sessionService;
    }
    public async Task<SessionResponse> Handle(CreateSessionFromAssignmentCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.UserId;
        Guard.Against.NullOrEmpty(userId, nameof(userId));
        var session = await _sessionService.CreateSessionAsync(request.AssignmentId, userId, cancellationToken);
        await _sessionService.SendOutSessionsAcitvityAsync(cancellationToken);

        return session;
    }
}

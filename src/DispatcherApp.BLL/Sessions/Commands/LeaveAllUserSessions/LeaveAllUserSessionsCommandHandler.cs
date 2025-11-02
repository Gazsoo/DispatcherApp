using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Common.Services;
using DispatcherApp.BLL.Sessions.Commands.LeaveSession;
using DispatcherApp.Common.Abstractions;
using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands.LeaveAllUserSessions;
internal sealed class LeaveAllUserSessionsCommandHandler (
    ISessionService sessionService,
    IUserContextService userContextService  
    ) : IRequestHandler<LeaveAllUserSessionsCommand, Unit>
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IUserContextService _userContextService = userContextService;
    public async Task<Unit> Handle(LeaveAllUserSessionsCommand request, CancellationToken cancellationToken)
    {

        var userId = _userContextService.UserId;
        Guard.Against.NullOrEmpty(userId, nameof(userId));
        await _sessionService.LeaveAllUserSessionsAsync(userId, cancellationToken);
        await _sessionService.SendOutSessionsAcitvityAsync(cancellationToken);
        return Unit.Value;
    }
}

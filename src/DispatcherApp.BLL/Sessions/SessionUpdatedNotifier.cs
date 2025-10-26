using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace DispatcherApp.BLL.Sessions;
public record SessionUpdatedNotification(string SessionId, long Version, DateTimeOffset UpdatedAt, string DataJson)
    : INotification;

internal sealed class SessionUpdatedNotifier(
    ISessionService sessionService,
    IUserContextService userContext
) : INotificationHandler<SessionUpdatedNotification>

{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IUserContextService _userContext = userContext;

    public async Task Handle(SessionUpdatedNotification n, CancellationToken ct)
    {
        var userId = _userContext.UserId;
        Guard.Against.NullOrEmpty(userId, nameof(userId));
        await _sessionService.UpdateSessionDataAsync(n.SessionId,n.Version,n.DataJson, userId, ct);
    }

}

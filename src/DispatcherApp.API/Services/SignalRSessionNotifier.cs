using DispatcherApp.API.Controllers;
using DispatcherApp.Common.Abstractions;
using DispatcherApp.Common.DTOs.Session;
using Microsoft.AspNetCore.SignalR;

namespace DispatcherApp.API.Services;

public sealed class SignalRSessionNotifier(
    IHubContext<SessionHub> _hub
) : ISessionNotifier
{
    public Task BroadcastUpdatedAsync(string sessionId, long version, SessionResponse dataJson, CancellationToken ct)
        => _hub.Clients.Group($"sess:{sessionId}")
              .SendAsync("SessionUpdated", dataJson, ct);
    public Task BrooadcastSessionsAcitvityAsync(SessionActivityResponse acivity, CancellationToken ct)
        => _hub.Clients.Group($"activity")
              .SendAsync("ActivityUpdate", acivity, ct);
}

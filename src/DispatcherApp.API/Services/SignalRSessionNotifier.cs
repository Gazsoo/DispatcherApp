using DispatcherApp.API.Controllers;
using DispatcherApp.Common.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace DispatcherApp.API.Services;

public sealed class SignalRSessionNotifier(
    IHubContext<SessionHub> _hub
) : ISessionNotifier
{
    public Task BroadcastUpdatedAsync(string sessionId, long version, string dataJson, CancellationToken ct)
        => _hub.Clients.Group($"sess:{sessionId}")
              .SendAsync("SessionUpdated", new { id = sessionId, version, data = dataJson }, ct);
}

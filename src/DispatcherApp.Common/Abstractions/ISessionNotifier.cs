using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.Common.DTOs.Session;

namespace DispatcherApp.Common.Abstractions;
public interface ISessionNotifier
{
    Task BroadcastUpdatedAsync(string sessionId, long version, SessionResponse response, CancellationToken ct);
    Task BrooadcastSessionsAcitvityAsync(SessionActivityResponse acivity, CancellationToken ct);

}

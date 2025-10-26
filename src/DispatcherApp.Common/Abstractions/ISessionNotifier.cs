using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.Abstractions;
public interface ISessionNotifier
{
    Task BroadcastUpdatedAsync(string sessionId, long version, string dataJson, CancellationToken ct);

}

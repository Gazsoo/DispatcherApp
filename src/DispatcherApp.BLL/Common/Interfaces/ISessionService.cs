using DispatcherApp.BLL.Sessions.Commands;
using DispatcherApp.Common.DTOs.Session;

namespace DispatcherApp.BLL.Common.Interfaces;

public interface ISessionService
{
    Task<SessionResponse> GetSessionDataAsync(string sessionId, CancellationToken ct = default);
    Task<IEnumerable<SessionResponse>> ListSessionsAsync(CancellationToken ct);
    Task<SessionResponse> UpdateSessionDataAsync(UpdateSessionCommand command,
        CancellationToken ct);
}

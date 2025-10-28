using DispatcherApp.BLL.Sessions.Commands.UpdateSession;
using DispatcherApp.Common.DTOs.Session;
using DispatcherApp.Common.Entities;

namespace DispatcherApp.BLL.Common.Interfaces;

public interface ISessionService
{
    Task<SessionResponse> JoinOrCreateAsync(string sessionId, string ownerUserId, CancellationToken ct = default);
    Task LeaveSessionAsync(string sessionId, string ownerUserId, CancellationToken ct = default);
    Task<SessionResponse> GetSessionDataAsync(int sessionId, CancellationToken ct = default);
    Task<IEnumerable<SessionResponse>> ListSessionsAsync(CancellationToken ct);
    Task<IEnumerable<SessionResponse>> ListActiveSessionsAsync(CancellationToken ct);
    Task<SessionResponse> UpdateSessionDataAsync(UpdateSessionCommand command,
        CancellationToken ct);
}

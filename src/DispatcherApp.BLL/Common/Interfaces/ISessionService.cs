using System.Threading;
using DispatcherApp.BLL.Sessions.Commands.UpdateSession;
using DispatcherApp.Common.Constants;
using DispatcherApp.Common.DTOs.Session;
using DispatcherApp.Common.Entities;

namespace DispatcherApp.BLL.Common.Interfaces;

public interface ISessionService
{
    Task SendOutSessionsAcitvityAsync(CancellationToken ct = default);
    Task<SessionResponse> CreateSessionAsync(int AssignmentId , string userId, CancellationToken ct = default);
    Task<SessionResponse> UpdateSessionStatusAsync(string sessionId,DispatcherSessionStatus status,CancellationToken ct = default);
    Task<SessionResponse> JoinOrCreateAsync(string sessionId, string currentUserId, CancellationToken ct = default);
    Task LeaveSessionAsync(string sessionId, string currentUserId, CancellationToken ct = default);
    Task LeaveAllUserSessionsAsync(string currentUserId, CancellationToken ct = default);
    Task<SessionResponse> GetSessionDataAsync(string sessionId, CancellationToken ct = default);
    Task<IEnumerable<SessionResponse>> ListSessionsAsync(CancellationToken ct);
    Task<IEnumerable<SessionResponse>> ListActiveSessionsAsync(CancellationToken ct);
    Task<SessionResponse> UpdateSessionDataAsync(UpdateSessionCommand command,
        CancellationToken ct);
}

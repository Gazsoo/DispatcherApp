using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.Common.DTOs.Session;
using DispatcherApp.Common.Entities;

namespace DispatcherApp.Common.Abstractions.Repository;
public interface ISessionRepository
{
    Task<DispatcherSession> AddParticipant(SessionParticipant user, string sessionId, CancellationToken ct = default);
    Task<DispatcherSession?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<int> AddLogFile(int logFileId, string sessionId, CancellationToken ct = default);
    Task<IEnumerable<DispatcherSession>> GetAll(CancellationToken ct = default);
    Task<IEnumerable<DispatcherSession>> GetActiveSessions(CancellationToken ct = default);
    Task<DispatcherSession?> GetBySessionIdAsync(string id, CancellationToken ct = default);
    void Update (DispatcherSession session);
    Task AddAsync(DispatcherSession session, CancellationToken ct = default);
    void Remove(DispatcherSession session);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<IEnumerable<DispatcherSession>> GetSessionsByUserIdAsync(string userId, CancellationToken ct = default);
}

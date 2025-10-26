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
    Task<DispatcherSession> GetByIdAsync(int id, CancellationToken ct = default);
    Task<DispatcherSession> GetBySessionIdAsync(string id, CancellationToken ct = default);
    Task<DispatcherSession> UpdateAsync (DispatcherSession session, CancellationToken ct = default);
    Task<DispatcherSession> AddAsync(DispatcherSession session, CancellationToken ct = default);
    void Remove(DispatcherSession session);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.Common.Entities;

namespace DispatcherApp.Common.Abstractions.Repository;
public interface ITutorialRepository
{
    Task<Tutorial?> GetByIdAsync(int id, bool includeFiles = false, CancellationToken ct = default);
    Task<List<Tutorial>> GetAllAsync(bool includeFiles = false, CancellationToken ct = default);
    Task AddAsync(Tutorial tutorial, CancellationToken ct = default);
    Task RemoveAsync(Tutorial tutorial, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

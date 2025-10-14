using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.Models.Entities;

namespace DispatcherApp.BLL.Common.Interfaces.Repository;
public interface ITutorialRepository
{
    Task<Tutorial?> GetByIdAsync(int id, bool includeFiles = false, CancellationToken ct = default);
    Task<List<Tutorial>> GetAllAsync(bool includeFiles = false, CancellationToken ct = default);
    Task AddAsync(Tutorial tutorial, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

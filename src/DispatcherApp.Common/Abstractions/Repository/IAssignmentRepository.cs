using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.Common.Entities;

namespace DispatcherApp.Common.Abstractions.Repository;

public interface IAssignmentRepository
{
    Task<Assignment?> GetByIdAsync(int id);
    Task<IEnumerable<Assignment>> GetAllAsync();
    Task AddAsync(Assignment assignment);
    Task SaveChangesAsync();
    Task<IEnumerable<Assignment>> GetUserAssignments(string userId);
}


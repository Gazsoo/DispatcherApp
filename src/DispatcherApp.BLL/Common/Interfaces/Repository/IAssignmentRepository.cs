using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.Models.Entities;

namespace DispatcherApp.BLL.Common.Interfaces.Repository;

public interface IAssignmentRepository
{
    Task<Assignment?> GetByIdAsync(int id);
    Task<IEnumerable<Assignment>> GetAllAsync();
    Task AddAsync(Assignment assignment);
    Task SaveChangesAsync();
    Task<IEnumerable<Assignment>> GetUserAssignments(string userId);
}


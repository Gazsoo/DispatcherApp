using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.DAL.Data;
using DispatcherApp.Common.Entities;
using Microsoft.EntityFrameworkCore;
using DispatcherApp.Common.Abstractions.Repository;

namespace DispatcherApp.DAL.Repositories;
public class AssignmentRepository : IAssignmentRepository
{
    private readonly AppDbContext _context;

    public AssignmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Assignment?> GetByIdAsync(int id)
    {
        return await _context.Assignments
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Assignment>> GetAllAsync()
    {
        return await _context.Assignments
            .AsNoTracking()
            .Include(a => a.AssignmentUsers)
            .ToListAsync();
    }

    public async Task AddAsync(Assignment assignment)
    {
        await _context.Assignments.AddAsync(assignment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Assignment>> GetUserAssignments(string userId)
    {
        return await _context.Assignments
            .AsNoTracking()
            .Include(a => a.AssignmentUsers)
            .Where(a => a.AssignmentUsers.Select(x => x.UserId).Contains(userId))
            .ToListAsync();
    }
}

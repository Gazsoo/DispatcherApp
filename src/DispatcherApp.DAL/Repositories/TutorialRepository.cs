using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.DAL.Data;
using DispatcherApp.Common.Entities;
using Microsoft.EntityFrameworkCore;
using DispatcherApp.Common.Abstractions.Repository;

namespace DispatcherApp.DAL.Repositories;
public class TutorialRepository : ITutorialRepository
{
    private readonly AppDbContext _context;

    public TutorialRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tutorial?> GetByIdAsync(int id, bool includeFiles = false, CancellationToken ct = default)
    {
        var query = _context.Tutorials.AsQueryable().AsSplitQuery();
        if (includeFiles)
            query = query.Include(t => t.Files).Include(t => t.Picture);

        return await query.FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<List<Tutorial>> GetAllAsync(bool includeFiles = false, CancellationToken ct = default)
    {
        var query = _context.Tutorials.AsQueryable().AsSplitQuery();
        if (includeFiles)
            query = query.Include(t => t.Files).Include(t => t.Picture);

        return await query.ToListAsync(ct);
    }

    public async Task AddAsync(Tutorial tutorial, CancellationToken ct = default)
    {
        await _context.Tutorials.AddAsync(tutorial, ct);
    }

    public Task RemoveAsync(Tutorial tutorial, CancellationToken ct = default)
    {
        _context.Tutorials.Remove(tutorial);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces.Repository;
using DispatcherApp.DAL.Data;
using DispatcherApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

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
        var query = _context.Tutorials.AsQueryable();
        if (includeFiles)
            query = query.Include(t => t.Files);

        return await query.FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<List<Tutorial>> GetAllAsync(bool includeFiles = false, CancellationToken ct = default)
    {
        var query = _context.Tutorials.AsQueryable();
        if (includeFiles)
            query = query.Include(t => t.Files);

        return await query.ToListAsync(ct);
    }

    public async Task AddAsync(Tutorial tutorial, CancellationToken ct = default)
    {
        await _context.Tutorials.AddAsync(tutorial, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.DAL.Data;
using DispatcherApp.Common.Entities;
using Microsoft.EntityFrameworkCore;
using File = DispatcherApp.Common.Entities.File;
using DispatcherApp.Common.Abstractions.Repository;

namespace DispatcherApp.DAL.Repositories;
public class FileRepository : IFileRepository
{
    private readonly AppDbContext _context;

    public FileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<File?> GetByIdAsync(int id)
        => await _context.Files.AsNoTracking().SingleOrDefaultAsync(f => f.Id == id);

    public async Task<List<File>> GetByIdsAsync(IEnumerable<int> ids)
        => await _context.Files.Where(f => ids.Contains(f.Id)).ToListAsync();

    public async Task<File> AddAsync(File file, CancellationToken ct = default)
    {
        await _context.Files.AddAsync(file, ct);
        return file; 
    }

    public void Remove(File file)
        => _context.Files.Remove(file);

    public void RemoveRange(IEnumerable<File> files)
        => _context.Files.RemoveRange(files);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task<List<File>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default)
    {
        return await _context.Files
            .Where(f => ids.Contains(f.Id))
            .AsNoTracking()
            .ToListAsync(ct);
    }
    public async Task<IEnumerable<File>> GetAllAsync()
    {
        return await _context.Files
            .AsNoTracking()
            .ToListAsync();
    }
}

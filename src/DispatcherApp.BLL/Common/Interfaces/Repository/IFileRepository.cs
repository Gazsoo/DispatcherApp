using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.API.Controllers;
using DispatcherApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using FileMetadata = DispatcherApp.Models.Entities.File;
namespace DispatcherApp.BLL.Common.Interfaces.Repository;
public interface IFileRepository
{
    Task<FileMetadata?> GetByIdAsync(int id);
    Task<List<FileMetadata>> GetByIdsAsync(IEnumerable<int> ids);
    Task<FileMetadata> AddAsync(FileMetadata file, CancellationToken ct = default);
    void Remove(FileMetadata file);
    void RemoveRange(IEnumerable<FileMetadata> files);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<IEnumerable<FileMetadata>> GetAllAsync();
    Task<List<FileMetadata>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);
}

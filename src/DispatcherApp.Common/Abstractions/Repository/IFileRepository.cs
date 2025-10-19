using DispatcherApp.Common.Entities;
using FileMetadata = DispatcherApp.Common.Entities.File;

namespace DispatcherApp.Common.Abstractions.Repository;
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

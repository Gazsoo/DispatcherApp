using DispatcherApp.BLL.Files.Commands.UploadFile;
using DispatcherApp.Common.DTOs.Files;
using Microsoft.AspNetCore.Mvc;
using File = DispatcherApp.Common.Entities.File;

namespace DispatcherApp.BLL.Common.Interfaces;

public interface IFileService
{
    Task<IEnumerable<File>> GetFilesMetadataAsync();
    Task<File> GetFileMetadataAsync(int id);
    Task<FileContentResult> GetFileAsync(int id);
    Task<FileUploadResponse> SaveFileAsync(
        FileUploadData fur,
        CancellationToken cancellationToken);
    Task DeleteFileAsync(int id);
    Task<IEnumerable<string>> DeleteMutipleFilesAsync(IEnumerable<int> ids, CancellationToken ct);
}

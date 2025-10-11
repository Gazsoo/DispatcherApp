using DispatcherApp.BLL.Files.Commands.DeleteFile;
using DispatcherApp.BLL.Files.Commands.UpdateFile;
using DispatcherApp.BLL.Model;
using DispatcherApp.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using File = DispatcherApp.Models.Entities.File;

namespace DispatcherApp.API.Controllers;

public interface IFileService
{
    Task<IEnumerable<File>> GetFilesMetadataAsync();
    Task<File> GetFileMetadataAsync(int id);
    Task<FileContentResult> GetFileAsync(int id);
    Task<FileUploadResponse> SaveFileAsync(
        FileUploadRequest fur,
        CancellationToken cancellationToken);
    Task DeleteFileAsync(int id);
    Task<IEnumerable<string>> DeleteMutipleFilesAsync(IEnumerable<int> ids, CancellationToken ct);
}

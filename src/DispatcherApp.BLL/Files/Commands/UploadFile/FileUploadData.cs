using System.IO;

namespace DispatcherApp.BLL.Files.Commands.UploadFile;
public record FileUploadData(
    Stream FileStream,
    string OriginalFileName,
    string ContentType,
    string Extension,
    string? Description)
{ }

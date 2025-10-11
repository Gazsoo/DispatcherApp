using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.BLL.Files.Commands.UpdateFile;
public record FileUploadRequest(
    Stream FileStream,
    string OriginalFileName,
    string ContentType,
    string Extension,
    string? Description)
{}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.Configurations;
public class FileStorageSettings
{
    public const string SectionName = "FileStorage";
    public string BlobUri { get; set; } = ""; // Azure Blob Storage URI
    public string ContainerName { get; set; } = "";
    public int MaxFileSize { get; set; } = 5 * 1024 * 1024; // 5 MB
    public string BasePath { get; set; } = ""; // Base path for file storage
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Models.Entities;
public class File
{
    public int Id { get; set; }
    public int TutorialId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string StoragePath { get; set; } = string.Empty; // Relative path or blob name
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Tutorial> Tutorials { get; set; } = new List<Tutorial>();
}

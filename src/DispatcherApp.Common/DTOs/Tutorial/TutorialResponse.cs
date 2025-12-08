using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.DTOs.Tutorial;
public class TutorialResponse
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? ContentType { get; set; }
    public FileResponse? Picture { get; set; }
    public ICollection<FileResponse> Files { get; set; } = new List<FileResponse>();
}

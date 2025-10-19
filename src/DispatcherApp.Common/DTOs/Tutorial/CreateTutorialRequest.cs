using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DispatcherApp.Common.DTOs.Tutorial;
public class CreateTutorialRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<int> CategoryIds { get; set; } = new();
    public int CreatedById { get; set; }
    public List<int> FilesId { get; set; } = new();
}

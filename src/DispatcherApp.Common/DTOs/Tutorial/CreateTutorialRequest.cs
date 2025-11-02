using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DispatcherApp.Common.DTOs.Tutorial;
public class CreateTutorialRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<int> CategoryIds { get; set; } = new();
    public string CreatedById { get; set; } = string.Empty;
    public List<int> FilesId { get; set; } = new();
}

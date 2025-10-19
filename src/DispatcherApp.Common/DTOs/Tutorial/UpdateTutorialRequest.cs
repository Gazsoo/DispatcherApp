using System.ComponentModel.DataAnnotations;

namespace DispatcherApp.Common.DTOs.Tutorial;

public class UpdateTutorialRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? ContentType { get; set; }
}

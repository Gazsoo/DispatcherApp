using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Models.DTOs.Tutorial;
public class CreateTutorialRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<int> CategoryIds { get; set; } = new();
    public int CreatedById { get; set; } = new();
    public List<int> FilesId { get; set; } = new();

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Models.DTOs.Tutorial;
public class FileResponse
{
    public int Id { get; set; }
    public required string  FileName { get; set; }
    public required string ContentType { get; set; }
    public long FileSize { get; set; }
}

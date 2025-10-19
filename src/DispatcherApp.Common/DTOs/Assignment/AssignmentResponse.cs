using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.DTOs.Assignment;
public class AssignmentResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; } 
    public string? Value { get; set; }
}

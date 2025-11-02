using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.Common.Entities;

namespace DispatcherApp.Common.DTOs.Assignment;
public class AssignmentResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; } 
    public string? Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PlannedTime { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Log { get; set; } = string.Empty;
    public AssignmentStatus Status { get; set; }
    public IEnumerable<UserResponse> Assignees { get; set; } = Enumerable.Empty<UserResponse>();

}

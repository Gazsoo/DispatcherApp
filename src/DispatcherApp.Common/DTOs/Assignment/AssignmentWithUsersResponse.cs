using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.DTOs.Assignment;
public class AssignmentWithUsersResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; } 
    public AssignmentStatus Status { get; set; }
    public string? Value { get; set; }
    public List<UserResponse> Assignees { get; set; } = new List<UserResponse>();
}

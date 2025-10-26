using System;
using System.Collections.Generic;
using DispatcherApp.Common.Entities;

namespace DispatcherApp.Common.DTOs.Assignment;

public class AssignmentCreateRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime PlannedTime { get; set; }
    public AssignmentStatus Status { get; set; } = AssignmentStatus.Pending;
    public string? Type { get; set; }
    public string? Value { get; set; }
    public IEnumerable<string> AssigneeIds { get; set; } = Array.Empty<string>();
}

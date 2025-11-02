using System;
namespace DispatcherApp.Common.DTOs.Assignment;

public class AssignmentUpdateRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? PlannedTime { get; set; }
    public string? Type { get; set; }
    public string? Value { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum AssignmentStatus
{
    Pending,
    InProgress,
    Completed,
    Cancelled
}
namespace DispatcherApp.Common.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime PlannedTime { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Log { get; set; } = string.Empty;
        public AssignmentStatus Status { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public List<string> AssigneeIds { get; set; } = new List<string>();
        public List<AssignmentUser> AssignmentUsers { get; set; } = new List<AssignmentUser>();

    }
}

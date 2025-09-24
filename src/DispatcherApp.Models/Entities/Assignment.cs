using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Models.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public List<string> AssigneeIds { get; set; } = new List<string>();
        public List<AssignmentUser> AssignmentUsers { get; set; } = new List<AssignmentUser>();

    }
}

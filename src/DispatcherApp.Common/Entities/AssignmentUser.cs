using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.Entities;
public class AssignmentUser
{
    public int AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }

    public required string UserId { get; set; }

    public DateTime AssignedAt { get; set; }
    public bool IsCompleted { get; set; }
}

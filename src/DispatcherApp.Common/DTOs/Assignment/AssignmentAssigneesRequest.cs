using System.Collections.Generic;

namespace DispatcherApp.Common.DTOs.Assignment;

public class AssignmentAssigneesRequest
{
    public IEnumerable<string> UserIds { get; set; } = new List<string>();
}

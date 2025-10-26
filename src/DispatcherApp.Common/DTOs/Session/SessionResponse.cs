using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.DTOs.Session;
public class SessionResponse
{
    public int AssignmentId { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public List<string> ParticipantIds { get; set; } = new List<string>();
    public string UserId { get; set; } = string.Empty;

}

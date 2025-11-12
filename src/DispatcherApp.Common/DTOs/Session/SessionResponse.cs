using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.Common.Constants;

namespace DispatcherApp.Common.DTOs.Session;
public class SessionResponse
{
    public string GroupId { get; set; } = string.Empty;
    public int AssignmentId { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public DispatcherSessionStatus Status { get; set; }
    public List<ParticipantDto> Participants { get; set; } = new();
    public string UserId { get; set; } = string.Empty;

}

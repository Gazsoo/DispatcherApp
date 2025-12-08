using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.Common.Constants;
using DispatcherApp.Common.DTOs.Files;

namespace DispatcherApp.Common.DTOs.Session;
public class SessionResponse
{
    public string GroupId { get; set; } = string.Empty;
    public int AssignmentId { get; set; }
    public FileMetadataResponse? LogFile {get; set;} 
    public string OwnerId { get; set; } = string.Empty;
    public DispatcherSessionStatus Status { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public List<ParticipantDto> Participants { get; set; } = new();
    public string UserId { get; set; } = string.Empty;

}

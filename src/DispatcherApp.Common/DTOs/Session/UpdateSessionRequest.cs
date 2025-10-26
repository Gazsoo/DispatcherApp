using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DispatcherApp.Common.Constants;

namespace DispatcherApp.Common.DTOs.Session;
public sealed record UpdateSessionRequest
{
    public string? OwnerId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? AssignmentId { get; set; }
    public DispatcherSessionType Type { get; set; }
    public DispatcherSessionStatus Status { get; set; }
    public List<ParticipantDto> Participants { get; set; } = new();
    public long IfMatchVersion { get; set; }
}

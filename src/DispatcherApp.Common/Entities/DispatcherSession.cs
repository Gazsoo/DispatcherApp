using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.Common.Abstractions.Repository;
using DispatcherApp.Common.Constants;
using Microsoft.AspNetCore.Identity;

namespace DispatcherApp.Common.Entities;

public class DispatcherSession : IVersionedEntity
{ 
    public int Id { get; set; }
    public string? OwnerId { get; set; }
    public string? GroupId { get; set; }
    public ICollection<SessionParticipant> Participants { get; set; } = new List<SessionParticipant>();
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public int? AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
    public DispatcherSessionType Type { get; set; }
    public DispatcherSessionStatus Status { get; set; }
    public long Version { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = default!;
}

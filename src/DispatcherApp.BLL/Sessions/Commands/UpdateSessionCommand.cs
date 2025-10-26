using DispatcherApp.Common.Constants;
using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands;
// Include properties to be used as input for the command
public record UpdateSessionCommand(
    string Id,
    string? OwnerId,
    DateTime StartTime,
    DateTime? EndTime,
    int? AssignmentId,
    DispatcherSessionType Type,
    DispatcherSessionStatus Status,
    ICollection<ParticipantDto> Participants,
    long IfMatchVersion
) : IRequest<SessionResponse>;

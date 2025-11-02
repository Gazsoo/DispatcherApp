using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.UpdateAssignmentStatus;

public sealed record UpdateAssignmentStatusCommand(int AssignmentId, AssignmentStatusUpdateRequest Request) : IRequest<AssignmentWithUsersResponse>;

using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.UpdateAssignment;

public sealed record UpdateAssignmentCommand(int AssignmentId, AssignmentUpdateRequest Request) : IRequest<AssignmentWithUsersResponse>;

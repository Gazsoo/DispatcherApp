using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.AddAssignmentAssignees;

public sealed record AddAssignmentAssigneesCommand(int AssignmentId, AssignmentAssigneesRequest Request) : IRequest<AssignmentWithUsersResponse>;

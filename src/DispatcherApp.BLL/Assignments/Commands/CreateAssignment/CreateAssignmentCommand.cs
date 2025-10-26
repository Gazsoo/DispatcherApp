using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.CreateAssignment;

public sealed record CreateAssignmentCommand(AssignmentCreateRequest Request) : IRequest<AssignmentWithUsersResponse>;

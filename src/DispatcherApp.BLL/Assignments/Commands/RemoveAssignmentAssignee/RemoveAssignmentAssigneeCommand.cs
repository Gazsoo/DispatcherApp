using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.RemoveAssignmentAssignee;

public sealed record RemoveAssignmentAssigneeCommand(int AssignmentId, string UserId) : IRequest<AssignmentWithUsersResponse>;

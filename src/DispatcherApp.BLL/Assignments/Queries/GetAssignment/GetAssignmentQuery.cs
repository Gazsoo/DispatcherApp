using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Queries.GetAssignment;

public sealed record GetAssignmentQuery(int AssignmentId) : IRequest<AssignmentWithUsersResponse>;

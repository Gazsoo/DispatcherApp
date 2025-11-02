using System.Collections.Generic;
using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Queries.GetMyAssignments;

public sealed record GetMyAssignmentsQuery() : IRequest<IEnumerable<AssignmentResponse>>;

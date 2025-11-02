using System.Collections.Generic;
using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Queries.GetAssignmentList;

public sealed record GetAssignmentListQuery() : IRequest<IEnumerable<AssignmentResponse>>;

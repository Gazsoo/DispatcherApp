using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Queries.GetMyAssignments;

internal sealed class GetMyAssignmentsQueryHandler : IRequestHandler<GetMyAssignmentsQuery, IEnumerable<AssignmentResponse>>
{
    private readonly IAssignmentService _assignmentService;

    public GetMyAssignmentsQueryHandler(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    public async Task<IEnumerable<AssignmentResponse>> Handle(GetMyAssignmentsQuery request, CancellationToken cancellationToken)
    {
        return await _assignmentService.GetUserAssignmentAsync(cancellationToken);
    }
}

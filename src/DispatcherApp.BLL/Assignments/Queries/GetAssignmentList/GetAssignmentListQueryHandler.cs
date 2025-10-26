using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Queries.GetAssignmentList;

internal sealed class GetAssignmentListQueryHandler : IRequestHandler<GetAssignmentListQuery, IEnumerable<AssignmentResponse>>
{
    private readonly IAssignmentService _assignmentService;

    public GetAssignmentListQueryHandler(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    public async Task<IEnumerable<AssignmentResponse>> Handle(GetAssignmentListQuery request, CancellationToken cancellationToken)
    {
        return await _assignmentService.GetAssignmentListAsync(cancellationToken);
    }
}

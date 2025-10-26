using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Queries.GetAssignment;

internal sealed class GetAssignmentQueryHandler : IRequestHandler<GetAssignmentQuery, AssignmentWithUsersResponse>
{
    private readonly IAssignmentService _assignmentService;

    public GetAssignmentQueryHandler(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    public async Task<AssignmentWithUsersResponse> Handle(GetAssignmentQuery request, CancellationToken cancellationToken)
    {
        return await _assignmentService.GetAssignmentAsync(request.AssignmentId, cancellationToken);
    }
}

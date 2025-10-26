using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.RemoveAssignmentAssignee;

internal sealed class RemoveAssignmentAssigneeCommandHandler : IRequestHandler<RemoveAssignmentAssigneeCommand, AssignmentWithUsersResponse>
{
    private readonly IAssignmentService _assignmentService;

    public RemoveAssignmentAssigneeCommandHandler(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    public async Task<AssignmentWithUsersResponse> Handle(RemoveAssignmentAssigneeCommand request, CancellationToken cancellationToken)
    {
        return await _assignmentService.RemoveAssigneeAsync(request.AssignmentId, request.UserId, cancellationToken);
    }
}

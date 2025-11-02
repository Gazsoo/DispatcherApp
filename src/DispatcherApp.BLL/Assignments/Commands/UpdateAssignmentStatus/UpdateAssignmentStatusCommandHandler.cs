using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.UpdateAssignmentStatus;

internal sealed class UpdateAssignmentStatusCommandHandler : IRequestHandler<UpdateAssignmentStatusCommand, AssignmentWithUsersResponse>
{
    private readonly IAssignmentService _assignmentService;

    public UpdateAssignmentStatusCommandHandler(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    public async Task<AssignmentWithUsersResponse> Handle(UpdateAssignmentStatusCommand request, CancellationToken cancellationToken)
    {
        return await _assignmentService.UpdateAssignmentStatusAsync(request.AssignmentId, request.Request.Status, cancellationToken);
    }
}

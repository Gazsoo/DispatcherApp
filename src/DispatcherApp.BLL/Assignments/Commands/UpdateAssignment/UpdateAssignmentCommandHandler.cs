using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.UpdateAssignment;

internal sealed class UpdateAssignmentCommandHandler : IRequestHandler<UpdateAssignmentCommand, AssignmentWithUsersResponse>
{
    private readonly IAssignmentService _assignmentService;

    public UpdateAssignmentCommandHandler(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    public async Task<AssignmentWithUsersResponse> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
    {
        return await _assignmentService.UpdateAssignmentAsync(request.AssignmentId, request.Request, cancellationToken);
    }
}

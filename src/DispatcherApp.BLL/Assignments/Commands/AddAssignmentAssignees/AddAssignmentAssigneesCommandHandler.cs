using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.AddAssignmentAssignees;

internal sealed class AddAssignmentAssigneesCommandHandler : IRequestHandler<AddAssignmentAssigneesCommand, AssignmentWithUsersResponse>
{
    private readonly IAssignmentService _assignmentService;

    public AddAssignmentAssigneesCommandHandler(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    public async Task<AssignmentWithUsersResponse> Handle(AddAssignmentAssigneesCommand request, CancellationToken cancellationToken)
    {
        return await _assignmentService.AddAssigneesAsync(request.AssignmentId, request.Request.UserIds, cancellationToken);
    }
}

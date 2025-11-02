using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Assignment;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.CreateAssignment;

internal sealed class CreateAssignmentCommandHandler : IRequestHandler<CreateAssignmentCommand, AssignmentWithUsersResponse>
{
    private readonly IAssignmentService _assignmentService;

    public CreateAssignmentCommandHandler(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    public async Task<AssignmentWithUsersResponse> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
    {
        return await _assignmentService.CreateAssignmentAsync(request.Request, cancellationToken);
    }
}

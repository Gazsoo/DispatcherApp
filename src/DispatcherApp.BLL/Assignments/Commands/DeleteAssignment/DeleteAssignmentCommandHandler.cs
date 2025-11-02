using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using MediatR;

namespace DispatcherApp.BLL.Assignments.Commands.DeleteAssignment;

internal sealed class DeleteAssignmentCommandHandler : IRequestHandler<DeleteAssignmentCommand, Unit>
{
    private readonly IAssignmentService _assignmentService;

    public DeleteAssignmentCommandHandler(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    public async Task<Unit> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
    {
        await _assignmentService.DeleteAssignmentAsync(request.AssignmentId, cancellationToken);
        return Unit.Value;
    }
}

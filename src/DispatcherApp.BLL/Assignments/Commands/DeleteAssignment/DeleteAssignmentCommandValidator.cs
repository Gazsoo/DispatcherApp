using FluentValidation;

namespace DispatcherApp.BLL.Assignments.Commands.DeleteAssignment;

public class DeleteAssignmentCommandValidator : AbstractValidator<DeleteAssignmentCommand>
{
    public DeleteAssignmentCommandValidator()
    {
        RuleFor(c => c.AssignmentId)
            .GreaterThan(0);
    }
}

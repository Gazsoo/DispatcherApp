using FluentValidation;

namespace DispatcherApp.BLL.Assignments.Commands.UpdateAssignment;

public class UpdateAssignmentCommandValidator : AbstractValidator<UpdateAssignmentCommand>
{
    public UpdateAssignmentCommandValidator()
    {
        RuleFor(c => c.AssignmentId)
            .GreaterThan(0);

        RuleFor(c => c.Request)
            .NotNull();

        RuleFor(c => c.Request.Name)
            .MaximumLength(200);
    }
}

using FluentValidation;

namespace DispatcherApp.BLL.Assignments.Commands.UpdateAssignmentStatus;

public class UpdateAssignmentStatusCommandValidator : AbstractValidator<UpdateAssignmentStatusCommand>
{
    public UpdateAssignmentStatusCommandValidator()
    {
        RuleFor(c => c.AssignmentId)
            .GreaterThan(0);

        RuleFor(c => c.Request)
            .NotNull();

        RuleFor(c => c.Request.Status)
            .IsInEnum();
    }
}

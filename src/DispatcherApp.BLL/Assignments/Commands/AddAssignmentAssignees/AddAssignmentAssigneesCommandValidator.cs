using FluentValidation;

namespace DispatcherApp.BLL.Assignments.Commands.AddAssignmentAssignees;

public class AddAssignmentAssigneesCommandValidator : AbstractValidator<AddAssignmentAssigneesCommand>
{
    public AddAssignmentAssigneesCommandValidator()
    {
        RuleFor(c => c.AssignmentId)
            .GreaterThan(0);

        RuleFor(c => c.Request)
            .NotNull();

        RuleFor(c => c.Request.UserIds)
            .NotEmpty();
    }
}

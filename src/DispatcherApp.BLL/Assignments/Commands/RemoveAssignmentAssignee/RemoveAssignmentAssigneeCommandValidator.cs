using FluentValidation;

namespace DispatcherApp.BLL.Assignments.Commands.RemoveAssignmentAssignee;

public class RemoveAssignmentAssigneeCommandValidator : AbstractValidator<RemoveAssignmentAssigneeCommand>
{
    public RemoveAssignmentAssigneeCommandValidator()
    {
        RuleFor(c => c.AssignmentId)
            .GreaterThan(0);

        RuleFor(c => c.UserId)
            .NotEmpty();
    }
}

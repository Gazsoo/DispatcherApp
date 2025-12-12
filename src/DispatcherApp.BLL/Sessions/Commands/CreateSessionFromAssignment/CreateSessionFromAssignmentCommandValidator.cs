using FluentValidation;

namespace DispatcherApp.BLL.Sessions.Commands.CreateSessionFromAssignment;
public class CreateSessionFromAssignmentCommandValidator : AbstractValidator<CreateSessionFromAssignmentCommand>
{
    public CreateSessionFromAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentId)
                    .NotEmpty()
                    .WithMessage("Assignment ID is required and cannot be empty.");
    }
}

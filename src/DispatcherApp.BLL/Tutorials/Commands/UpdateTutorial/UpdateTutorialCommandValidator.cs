using FluentValidation;

namespace DispatcherApp.BLL.Tutorials.Commands.UpdateTutorial;

public sealed class UpdateTutorialCommandValidator : AbstractValidator<UpdateTutorialCommand>
{
    public UpdateTutorialCommandValidator()
    {
        RuleFor(x => x.TutorialId)
            .GreaterThan(0);

        RuleFor(x => x.Request)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Request!.Title)
                    .NotEmpty()
                    .WithMessage("Title is required.");
            });
    }
}

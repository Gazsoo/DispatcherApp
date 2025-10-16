using FluentValidation;

namespace DispatcherApp.BLL.Tutorials.Commands.CreateTutorial;

public sealed class CreateTutorialCommandValidator : AbstractValidator<CreateTutorialCommand>
{
    public CreateTutorialCommandValidator()
    {
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

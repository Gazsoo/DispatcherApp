using FluentValidation;

namespace DispatcherApp.BLL.Files.Commands.DeleteFile;
public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
{
    public DeleteFileCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("File ID must be greater than zero.");
    }
}

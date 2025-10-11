using FluentValidation;

namespace DispatcherApp.BLL.Files.Commands.DeleteFiles;
public class DeleteFilesCommandValidator : AbstractValidator<DeleteFilesCommand>
{
    public DeleteFilesCommandValidator()
    {
        RuleForEach(x => x.Ids)
            .GreaterThan(0)
            .WithMessage("Each File ID must be greater than zero.");
    }
}

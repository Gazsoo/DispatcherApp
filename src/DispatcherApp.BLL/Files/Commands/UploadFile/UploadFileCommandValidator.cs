using DispatcherApp.Common.Configurations;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace DispatcherApp.BLL.Files.Commands.UploadFile;
public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    private readonly FileStorageSettings _settings;

    public UploadFileCommandValidator(IOptions<FileStorageSettings> fileSettings)
    {
        _settings = fileSettings.Value;

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.");

        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(_settings.MaxFileSize)
            .WithMessage($"File must not exceed {_settings.MaxFileSize} bytes.");
    }
}

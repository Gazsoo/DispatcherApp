using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Configurations;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace DispatcherApp.BLL.Files.Commands;
public class UploadFileValidator : AbstractValidator<UploadFileCommand>
{
    private readonly FileStorageSettings  _settings;
    public UploadFileValidator(IOptions<FileStorageSettings> fileSettings)
    {
        _settings = fileSettings.Value;

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.");

        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(_settings.MaxFileSize)
            .WithMessage($"File must not exceed {_settings.MaxFileSize}Byte");
    }
}

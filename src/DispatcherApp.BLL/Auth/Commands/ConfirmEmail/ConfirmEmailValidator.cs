using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace DispatcherApp.BLL.Auth.Commands.ConfirmEmail;

public sealed class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required and cannot be empty.");
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token is required and cannot be empty.");
    }
}

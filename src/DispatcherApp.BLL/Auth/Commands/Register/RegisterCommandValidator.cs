using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using DispatcherApp.Common.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace DispatcherApp.BLL.Auth.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator(UserManager<IdentityUser> userManager)
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, cancellationToken) =>
            {
                var existingUser = await userManager.FindByEmailAsync(email);
                return existingUser == null;
            })
            .WithMessage("An account with this email already exists.");

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.Request.Role)
            .Must(role => string.IsNullOrWhiteSpace(role) || Roles.All.Contains(role))
            .WithMessage("Invalid role specified.");
    }
}

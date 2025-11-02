using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace DispatcherApp.BLL.Auth.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator(UserManager<IdentityUser> userManager)
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Request.Password)
            .NotEmpty();

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
            {
                var user = await userManager.FindByEmailAsync(command.Request.Email);
                if (user is null)
                {
                    return true;
                }

                return await userManager.IsEmailConfirmedAsync(user);
            })
            .WithName("Email")
            .WithMessage("Email has not been confirmed. Please verify your account.")
            .When(x => !string.IsNullOrWhiteSpace(x.Request.Email));
    }
}

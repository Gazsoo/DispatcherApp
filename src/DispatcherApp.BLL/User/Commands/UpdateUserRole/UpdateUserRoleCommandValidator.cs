using DispatcherApp.Common.Constants;
using FluentValidation;

namespace DispatcherApp.BLL.User.Commands.UpdateUserRole;
public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty();

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(r => Roles.All.Contains(r))
            .WithMessage($"Role must be one of: {string.Join(", ", Roles.All)}");
    }
}

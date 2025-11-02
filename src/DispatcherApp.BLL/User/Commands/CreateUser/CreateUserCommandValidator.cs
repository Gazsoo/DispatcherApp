using FluentValidation;

namespace DispatcherApp.BLL.User.Commands.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        // Add validation rules here
    }
}

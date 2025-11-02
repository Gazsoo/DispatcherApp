using FluentValidation;

namespace DispatcherApp.BLL.User.Commands.DeleteUser;
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        // Add validation rules here
    }
}

using FluentValidation;

namespace DispatcherApp.BLL.Sessions.Commands.LeaveSession;
public class LeaveSessionCommandValidator : AbstractValidator<LeaveSessionCommand>
{
    public LeaveSessionCommandValidator()
    {
        // Add validation rules here
    }
}

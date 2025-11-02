using FluentValidation;

namespace DispatcherApp.BLL.Sessions.Commands.LeaveAllUserSessions;
public class LeaveAllUserSessionsCommandValidator : AbstractValidator<LeaveAllUserSessionsCommand>
{
    public LeaveAllUserSessionsCommandValidator()
    {
        // Add validation rules here
    }
}

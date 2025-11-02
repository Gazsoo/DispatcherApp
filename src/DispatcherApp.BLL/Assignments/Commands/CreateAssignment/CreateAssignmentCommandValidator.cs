using System;
using DispatcherApp.Common.DTOs.Assignment;
using FluentValidation;

namespace DispatcherApp.BLL.Assignments.Commands.CreateAssignment;

public class CreateAssignmentCommandValidator : AbstractValidator<CreateAssignmentCommand>
{
    public CreateAssignmentCommandValidator()
    {
        RuleFor(c => c.Request)
            .NotNull();

        RuleFor(c => c.Request.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(c => c.Request.PlannedTime)
            .GreaterThan(DateTime.MinValue)
            .WithMessage("Planned time must be provided.");
    }
}

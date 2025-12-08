using System;
using DispatcherApp.Common.DTOs.Assignment;
using FluentValidation;

namespace DispatcherApp.BLL.Sessions.Commands.GetOrCreateSessionCommand;

public class GetOrCreateSessionCommandValidator : AbstractValidator<JoinGetOrCreateSessionCommand>
{
    public GetOrCreateSessionCommandValidator()
    {

    }
}

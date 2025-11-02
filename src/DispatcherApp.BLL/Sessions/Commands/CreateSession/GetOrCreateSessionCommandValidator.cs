using System;
using DispatcherApp.Common.DTOs.Assignment;
using FluentValidation;

namespace DispatcherApp.BLL.Sessions.Commands.CreateSession;

public class GetOrCreateSessionCommandValidator : AbstractValidator<JoinGetOrCreateSessionCommand>
{
    public GetOrCreateSessionCommandValidator()
    {

    }
}

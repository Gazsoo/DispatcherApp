using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.ResendConfirmationEmail;

public sealed record ResendConfirmationEmailCommand(string Email, string ConfirmationBaseUrl) : IRequest<bool>;

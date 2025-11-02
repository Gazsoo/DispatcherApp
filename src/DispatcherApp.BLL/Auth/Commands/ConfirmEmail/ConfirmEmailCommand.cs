using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(string UserId, string Token) : IRequest<bool>;

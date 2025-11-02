using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email, string ResetBaseUrl) : IRequest<bool>;

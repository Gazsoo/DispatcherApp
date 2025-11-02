using DispatcherApp.Common.DTOs.Auth;
using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.ResetPassword;

public sealed record ResetPasswordCommand(ResetPasswordRequest Request) : IRequest<bool>;

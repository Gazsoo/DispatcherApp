using DispatcherApp.Common.DTOs.Auth;
using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.Register;

public sealed record RegisterCommand(RegisterRequest Request, string ConfirmationBaseUrl) : IRequest<bool>;

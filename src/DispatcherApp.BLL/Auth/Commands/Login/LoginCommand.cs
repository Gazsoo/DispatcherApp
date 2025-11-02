using DispatcherApp.Common.DTOs.Auth;
using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.Login;

public sealed record LoginCommand(LoginRequest Request) : IRequest<AuthResponse?>;

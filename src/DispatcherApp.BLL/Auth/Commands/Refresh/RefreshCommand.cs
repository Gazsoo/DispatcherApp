using DispatcherApp.Common.DTOs.Auth;
using MediatR;

namespace DispatcherApp.BLL.Auth.Commands.Refresh;

public sealed record RefreshCommand(RefreshRequest Request) : IRequest<AuthResponse?>;

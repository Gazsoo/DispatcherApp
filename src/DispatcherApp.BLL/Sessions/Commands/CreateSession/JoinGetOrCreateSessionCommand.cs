using DispatcherApp.Common.DTOs.Assignment;
using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands.CreateSession;

public sealed record JoinGetOrCreateSessionCommand(string SessionId) : IRequest<SessionResponse>;

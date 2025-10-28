using System.Data;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.Abstractions.Repository;
using DispatcherApp.Common.DTOs.Session;
using DispatcherApp.Common.Exceptions;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Commands.UpdateSession;
internal sealed class UpdateSessionCommandHandler(
    ISessionService sessionService
) : IRequestHandler<UpdateSessionCommand, SessionResponse>
{
    private readonly ISessionService _sessionService = sessionService;
    public async Task<SessionResponse> Handle(UpdateSessionCommand request, CancellationToken ct)
    {
        return await _sessionService.UpdateSessionDataAsync(request, ct);

    }
}

using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Queries.GetSession;
internal sealed class GetSessionQueryHandler(ISessionService sessionServie) : IRequestHandler<GetSessionQuery, SessionResponse>
{
    private readonly ISessionService _sessionService = sessionServie;

    public async Task<SessionResponse> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        return await _sessionService.GetSessionDataAsync(request.SessionId, cancellationToken);
    }
}

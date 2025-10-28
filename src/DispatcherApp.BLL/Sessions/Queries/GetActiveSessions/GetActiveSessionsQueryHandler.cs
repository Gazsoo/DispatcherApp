using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Queries.GetActiveSessions;
internal sealed class GetActiveSessionsQueryHandler (ISessionService sessionService): IRequestHandler<GetActiveSessionsQuery, IEnumerable<SessionResponse>>
{
    private readonly ISessionService _sessionService = sessionService;
    public async Task<IEnumerable<SessionResponse>> Handle(GetActiveSessionsQuery request, CancellationToken cancellationToken)
    {
       return await _sessionService.ListActiveSessionsAsync(cancellationToken);
    }
}

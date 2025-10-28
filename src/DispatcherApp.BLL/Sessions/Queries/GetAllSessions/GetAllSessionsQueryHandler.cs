using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Session;
using MediatR;

namespace DispatcherApp.BLL.Sessions.Queries.GetAllSessions;
internal sealed class GetAllSessionsQueryHandler (ISessionService sessionService): IRequestHandler<GetAllSessionsQuery, IEnumerable<SessionResponse>>
{
    private readonly ISessionService _sessionService = sessionService;
    public async Task<IEnumerable<SessionResponse>> Handle(GetAllSessionsQuery request, CancellationToken cancellationToken)
    {
        return await _sessionService.ListSessionsAsync(cancellationToken);
    }
}

using DispatcherApp.BLL.Sessions.Commands;
using DispatcherApp.BLL.Sessions.Queries;
using DispatcherApp.Common.DTOs.Session;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DispatcherApp.API.Controllers;

//[Authorize]
public class SessionHub : Hub
{
    private readonly IMediator _mediator;
    public SessionHub(IMediator mediator) => _mediator = mediator;

    public Task<string> Ping() => Task.FromResult("pong");

    // helpful sanity checks
    public Task<string> WhoAmI()
    {

        return Task.FromResult($"{Context.User?.Identity?.Name} hte cudd");
    }
    public Task<SessionResponse> GetSession(string sessionId)
        => _mediator.Send(new GetSessionQuery(sessionId), Context.ConnectionAborted);

    public Task JoinSession(string sessionId)
    => Groups.AddToGroupAsync(Context.ConnectionId, $"sess:{sessionId}");

    public Task LeaveSession(string sessionId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, $"sess:{sessionId}");

    // Optional WS write path (instead of HTTP PUT)
    public Task<SessionResponse> UpdateSession(string sessionId, long ifMatchVersion, string dataJson)
        => _mediator.Send(new UpdateSessionCommand(sessionId, ifMatchVersion, dataJson), Context.ConnectionAborted);

}

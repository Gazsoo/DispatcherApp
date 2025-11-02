using DispatcherApp.BLL.Sessions.Commands;
using DispatcherApp.BLL.Sessions.Commands.CreateSession;
using DispatcherApp.BLL.Sessions.Commands.LeaveAllUserSessions;
using DispatcherApp.BLL.Sessions.Commands.LeaveSession;
using DispatcherApp.BLL.Sessions.Commands.UpdateSession;
using DispatcherApp.BLL.Sessions.Queries;
using DispatcherApp.BLL.Sessions.Queries.GetActiveSessions;
using DispatcherApp.BLL.Sessions.Queries.GetAllSessions;
using DispatcherApp.BLL.Sessions.Queries.GetSession;
using DispatcherApp.Common.DTOs.Session;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DispatcherApp.API.Controllers;

[Authorize]
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
    public Task<SessionResponse> GetSession(int sessionId)
        => _mediator.Send(new GetSessionQuery(sessionId), Context.ConnectionAborted);

    public async Task JoinSession(string sessionId)
    {
        // Try to get or create the session through MediatR
        var session = await _mediator.Send(new JoinGetOrCreateSessionCommand(sessionId), Context.ConnectionAborted);

        // Now attach this connection to the SignalR group
        await Groups.AddToGroupAsync(Context.ConnectionId, $"sess:{session.GroupId}");

        // Optionally tell the caller what happened
        //await Clients.Caller.SendAsync("SessionJoined", new { id = session.GroupId });
        //return await _mediator.Send(new GetAllSessionsQuery());
    }
    public async Task<IEnumerable<SessionResponse>> JoinActivityGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"activity");

        // Optionally tell the caller what happened
        await Clients.Caller.SendAsync("SessionJoined", new { id = "ACITIVE" });
        return await _mediator.Send(new GetActiveSessionsQuery());
    }
    public async Task LeaveSession(string sessionId)
    {
        await _mediator.Send(new LeaveSessionCommand(sessionId), Context.ConnectionAborted);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"sess:{sessionId}");
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _mediator.Send(new LeaveAllUserSessionsCommand());
        await base.OnDisconnectedAsync(exception);
    }
    //// Optional WS write path (instead of HTTP PUT)
    //public Task<SessionResponse> UpdateSession(string sessionId, long ifMatchVersion, string dataJson)
    //    => _mediator.Send(
    //        new UpdateSessionCommand(
    //            sessionId, ifMatchVersion, dataJson), 
    //        Context.ConnectionAborted);

}

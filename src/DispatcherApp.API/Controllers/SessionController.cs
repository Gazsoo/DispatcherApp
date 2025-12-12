using DispatcherApp.BLL.Sessions.Commands;
using DispatcherApp.BLL.Sessions.Commands.CreateSessionFromAssignment;
using DispatcherApp.BLL.Sessions.Commands.GetOrCreateSessionCommand;
using DispatcherApp.BLL.Sessions.Commands.LeaveSession;
using DispatcherApp.BLL.Sessions.Commands.UpdateSession;
using DispatcherApp.BLL.Sessions.Commands.UpdateSessionState;
using DispatcherApp.BLL.Sessions.Queries;
using DispatcherApp.BLL.Sessions.Queries.GetActiveSessions;
using DispatcherApp.BLL.Sessions.Queries.GetAllSessions;
using DispatcherApp.BLL.Sessions.Queries.GetSession;
using DispatcherApp.Common.Constants;
using DispatcherApp.Common.DTOs.Session;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DispatcherApp.API.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SessionController (IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet()]
    [Authorize(Roles = "Dispatcher,Administrator,User")]
    public async Task<ActionResult<IEnumerable<SessionResponse>>> Get()
    {
        return Ok(await _mediator.Send(new GetAllSessionsQuery()));
    }
    [HttpGet("active")]
    [Authorize(Roles = "Dispatcher,Administrator,User")]
    public async Task<ActionResult<IEnumerable<SessionResponse>>> GetActive()
    {
        return Ok( await _mediator.Send(new GetActiveSessionsQuery()));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Dispatcher,Administrator,User")]
    public async Task<ActionResult<SessionResponse>> Get(string id)
    {
        return Ok(await _mediator.Send(new GetSessionQuery(id)));
    }

    [HttpPatch("{id}/{status}")]
    [Authorize(Roles = "Dispatcher,Administrator,User")]
    public async Task<ActionResult<SessionResponse>> UpdateStatus(string id, DispatcherSessionStatus status)
    {
        return Ok(await _mediator.Send(new UpdateSessionStateCommand(id, status)));
    }
    [HttpGet("{id}/join")]
    [Authorize(Roles = "Dispatcher,Administrator,User")]
    public async Task<ActionResult<SessionResponse>> Join(string id)
    {
        return await _mediator.Send(new JoinGetOrCreateSessionCommand(id));
    }

    [HttpPost("{id}/leave")]
    [Authorize(Roles = "Dispatcher,Administrator,User")]
    public async Task<ActionResult> Leave(string id, int logFileId)
    {
        await _mediator.Send(new LeaveSessionCommand(id, logFileId));
        return Ok();
    }
    [HttpPost]
    [Authorize(Roles = "Dispatcher,Administrator,User")]
    public async Task<ActionResult<SessionResponse>> CreateSession(int assignmentId)
    {
        return await _mediator.Send(new CreateSessionFromAssignmentCommand(assignmentId));
    }

    // PUT api/<SessionController>/5
    [HttpPut("{sessionId}")]
    [Authorize(Roles = "Dispatcher,Administrator,User")]
    public async Task<IActionResult> UpdateSession(string id, [FromBody] UpdateSessionRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateSessionCommand(
            id,
            req.OwnerId,
            req.StartTime,
            req.EndTime,
            req.AssignmentId,
            req.Type,
            req.Status,
            req.Participants,
            req.IfMatchVersion
        ), ct);

        return Ok(result);
    }

}

using DispatcherApp.BLL.Sessions.Commands;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DispatcherApp.API.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SessionController (IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/<SessionController>
    [HttpGet()]
    public async Task<ActionResult<IEnumerable<SessionResponse>>> Get()
    {
        return Ok(await _mediator.Send(new GetAllSessionsQuery()));
    }
    // GET: api/<SessionController>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<SessionResponse>>> GetActive()
    {
        return Ok( await _mediator.Send(new GetActiveSessionsQuery()));
    }

    // GET api/<SessionController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SessionResponse>> Get(int id)
    {
        return Ok(await _mediator.Send(new GetSessionQuery(id)));
    }
    [HttpPatch("{id}/{status}")]
    public async Task<ActionResult<SessionResponse>> UpdateStatus(string sessionId, DispatcherSessionStatus dss)
    {
        return Ok(await _mediator.Send(new UpdateSessionStateCommand(sessionId, dss)));
    }
    // POST api/<SessionController>
    //[HttpPost]
    //public void Post([FromBody] SessionCreateRequest request)
    //{
    //    return await _mediator.Send(new CreateSessionCommand
    //    {

    //    });
    //}

    // PUT api/<SessionController>/5
    [HttpPut("{sessionId}")]
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

    // DELETE api/<SessionController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}

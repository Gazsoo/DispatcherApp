using DispatcherApp.BLL.Sessions.Commands;
using DispatcherApp.BLL.Sessions.Queries;
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

    [HttpPost("{id}/_simulate-update")]
    #if DEBUG
        public async Task<IActionResult> SimulateUpdate(string id, [FromBody] UpdateSessionRequest body, CancellationToken ct)
        {
            var dto = await _mediator.Send(new UpdateSessionCommand(id, body.IfMatchVersion, body.Data.ToString()), ct);
            return Ok(dto);
        }
    #else
    public IActionResult SimulateUpdate() => NotFound();
    #endif
    // GET: api/<SessionController>
    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<SessionResponse>>> Get()
    //{
    //   return await _mediator.Send(new GetSessionQuery);
    //}

    // GET api/<SessionController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SessionResponse>> Get(string sessionId)
    {
        return await _mediator.Send(new GetSessionQuery(sessionId));
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
    public async Task<IActionResult> UpdateSession(int id, [FromBody] UpdateSessionRequest req, CancellationToken ct)
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

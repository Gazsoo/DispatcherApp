using DispatcherApp.BLL.Assignments.Commands.AddAssignmentAssignees;
using DispatcherApp.BLL.Assignments.Commands.DeleteAssignment;
using DispatcherApp.BLL.Assignments.Commands.RemoveAssignmentAssignee;
using DispatcherApp.BLL.Assignments.Commands.UpdateAssignment;
using DispatcherApp.BLL.Assignments.Commands.UpdateAssignmentStatus;
using DispatcherApp.BLL.Assignments.Queries.GetAssignment;
using DispatcherApp.BLL.Assignments.Queries.GetAssignmentList;
using DispatcherApp.BLL.Assignments.Queries.GetMyAssignments;
using DispatcherApp.Common.DTOs.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DispatcherApp.BLL.Assignments.Commands.CreateAssignment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DispatcherApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AssignmentController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [Authorize(Roles = "Dispatcher,Administrator,User")]
    public async Task<ActionResult<IEnumerable<AssignmentResponse>>> GetAssignments()
    {
        var result = await _mediator.Send(new GetAssignmentListQuery());
        return Ok(result);
    }
    [HttpGet("my")]
    [Authorize(Roles = "Dispatcher,Administrator,User")]
    public async Task<ActionResult<IEnumerable<AssignmentResponse>>> GetMyAssignments()
    {
        var result = await _mediator.Send(new GetMyAssignmentsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Dispatcher,Administrator,User")]
    public async Task<ActionResult<AssignmentWithUsersResponse>> GetAssignment(int id)
    {
        var result = await _mediator.Send(new GetAssignmentQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Dispatcher,Administrator")]
    public async Task<ActionResult<AssignmentWithUsersResponse>> CreateAssignment([FromBody] AssignmentCreateRequest request)
    {
        var result = await _mediator.Send(new CreateAssignmentCommand(request));
        return CreatedAtAction(nameof(GetAssignment), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Dispatcher,Administrator")]
    public async Task<ActionResult<AssignmentWithUsersResponse>> UpdateAssignment(int id, [FromBody] AssignmentUpdateRequest request)
    {
        var result = await _mediator.Send(new UpdateAssignmentCommand(id, request));
        return Ok(result);
    }

    [HttpPost("{id}/status")]
    public async Task<ActionResult<AssignmentWithUsersResponse>> UpdateAssignmentStatus(int id, [FromBody] AssignmentStatusUpdateRequest request)
    {
        var result = await _mediator.Send(new UpdateAssignmentStatusCommand(id, request));
        return Ok(result);
    }

    [HttpPost("{id}/assignees")]
    [Authorize(Roles = "Dispatcher,Administrator")]
    public async Task<ActionResult<AssignmentWithUsersResponse>> AddAssignees(int id, [FromBody] AssignmentAssigneesRequest request)
    {
        var result = await _mediator.Send(new AddAssignmentAssigneesCommand(id, request));
        return Ok(result);
    }

    [HttpDelete("{id}/assignees/{userId}")]
    [Authorize(Roles = "Dispatcher,Administrator")]
    public async Task<ActionResult<AssignmentWithUsersResponse>> RemoveAssignee(int id, string userId)
    {
        var result = await _mediator.Send(new RemoveAssignmentAssigneeCommand(id, userId));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Dispatcher,Administrator")]
    public async Task<IActionResult> DeleteAssignment(int id)
    {
        await _mediator.Send(new DeleteAssignmentCommand(id));
        return NoContent();
    }
}

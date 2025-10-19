using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DispatcherApp.API.Controllers;
[Authorize]
[Authorize(Roles = "Dispatcher,Administrator")]
[Route("api/[controller]")]
[ApiController]
public class AssignmentController (IAssignmentService assignmentService) : ControllerBase
{
    private readonly IAssignmentService _assignmentService = assignmentService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssignmentResponse>>> GetAssignments()
    {
        var result = await _assignmentService.GetAssignmentListAsync();
        return Ok(result);
    }
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<AssignmentResponse>>> GetMyAssignments()
    {
        var result = await _assignmentService.GetUserAssignmentAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<AssignmentResponse>>> GetAssignment(int id)
    {
        var result = await _assignmentService.GetAssignmentAsync(id);
        return Ok(result);
    }

    // POST api/<AssignmentController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<AssignmentController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<AssignmentController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}

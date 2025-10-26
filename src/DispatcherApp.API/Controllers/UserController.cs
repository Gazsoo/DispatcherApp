using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.User.Commands.CreateUser;
using DispatcherApp.BLL.User.Queries.GetAllUsers;
using DispatcherApp.BLL.User.Queries.GetUser;
using DispatcherApp.Common.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DispatcherApp.API.Controllers;
[Route("api/[controller]")]
[Authorize(Roles = "Administrator")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator ;
    private readonly IAssignmentService _assignmentService;
    public UserController(IAssignmentService assignmentService, IMediator mediator)
    {
        _assignmentService = assignmentService;
        _mediator = mediator;
    }

    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var a = _assignmentService.GetCurrentUserId();
        return Ok(new { username = "sampleuser", email = a?.Roles });
    }

    [HttpGet("User")]
    public async Task<ActionResult<UserInfoResponse>> GetUser(string id)
    {
        return await _mediator.Send(new GetUserQuery(id));
    }

    [HttpGet("AllUser")]
    public async Task<ActionResult<GetAllUsersResponse>> GetAllUsers(string id)
    {
        return await _mediator.Send(new GetAllUsersQuery());
    }

    [HttpPost("User")]
    public async Task<ActionResult<UserInfoResponse>> CreateUser([FromBody]CreateUserRequest request)
    {
        return await _mediator.Send(new CreateUserCommand(request));
    }
}

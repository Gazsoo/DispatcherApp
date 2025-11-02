using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.User.Commands.CreateUser;
using DispatcherApp.BLL.User.Queries.GetAllUsers;
using DispatcherApp.BLL.User.Queries.GetProfile;
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
    public async Task<ActionResult<UserInfoResponse>> GetProfile()
    {
        return await _mediator.Send(new GetProfileQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserInfoResponse>> GetUser(string id)
    {
        return await _mediator.Send(new GetUserQuery(id));
    }

    [HttpGet("AllUser")]
    public async Task<ActionResult<GetAllUsersResponse>> GetAllUsers()
    {
        return await _mediator.Send(new GetAllUsersQuery());
    }

    [HttpPost]
    public async Task<ActionResult<UserInfoResponse>> CreateUser([FromBody]CreateUserRequest request)
    {
        return await _mediator.Send(new CreateUserCommand(request));
    }
}

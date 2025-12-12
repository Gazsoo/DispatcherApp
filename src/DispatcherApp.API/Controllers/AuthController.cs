using System.Security.Claims;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Auth.Commands.Login;
using DispatcherApp.BLL.Auth.Commands.Register;
using DispatcherApp.BLL.Auth.Commands.ConfirmEmail;
using DispatcherApp.BLL.Auth.Queries.GetUserInfo;
using DispatcherApp.BLL.Auth.Commands.UpdateUserInfo;
using DispatcherApp.BLL.Auth.Commands.Refresh;
using DispatcherApp.Common.DTOs.Auth;
using DispatcherApp.Common.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace DispatcherApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(
        IMediator mediator
        )
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        var confirmationBaseUrl = Url.Action("ConfirmEmail", "Auth", null, Request.Scheme);
        Guard.Against.Null(confirmationBaseUrl, nameof(confirmationBaseUrl));
        var success = await _mediator.Send(new RegisterCommand(request, confirmationBaseUrl));
        return Ok();
    }

    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpGet("confirmEmail")]
    public async Task<ActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var success = await _mediator.Send(new ConfirmEmailCommand(userId, token));
        return Ok("Confirmed");
    }

    [HttpGet("manage/info")]
    [Authorize]
    public async Task<ActionResult<UserInfoResponse>> GetUserInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var userInfo = await _mediator.Send(new GetUserInfoQuery(userId));
        return Ok(userInfo);
    }

    [HttpPost("manage/info")]
    [Authorize]
    public async Task<ActionResult<UserInfoResponse>> UpdateUserInfo([FromBody] UserInfoResponse request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        var updatedUser = await _mediator.Send(new UpdateUserInfoCommand(userId, request));
        return Ok(updatedUser);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {

        var result = await _mediator.Send(new LoginCommand(request));
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshRequest request)
    {
        var result = await _mediator.Send(new RefreshCommand(request));
        return Ok(result);
    }
}

using System.Security.Claims;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Auth.Commands.Login;
using DispatcherApp.BLL.Auth.Commands.Register;
using DispatcherApp.BLL.Auth.Commands.ConfirmEmail;
using DispatcherApp.BLL.Auth.Commands.ResendConfirmationEmail;
using DispatcherApp.BLL.Auth.Commands.ForgotPassword;
using DispatcherApp.BLL.Auth.Commands.ResetPassword;
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
        if (!success)
            return BadRequest("Registration failed");

        return Ok(new { message = "Confirmation email sent if your account exists." });
    }

    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpGet("confirmEmail")]
    public async Task<ActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            return BadRequest("Invalid confirmation link");

        var success = await _mediator.Send(new ConfirmEmailCommand(userId, token));
        if (!success)
            return BadRequest("Email confirmation failed");

        return Ok(new { message = "Email confirmed successfully" });
    }

    [HttpPost("resendConfirmationEmail")]
    public async Task<ActionResult> ResendConfirmationEmail([FromBody] EmailRequest request)
    {
        var confirmationBaseUrl = Url.Action("ConfirmEmail", "Auth", null, Request.Scheme);
        Guard.Against.Null(confirmationBaseUrl, nameof(confirmationBaseUrl));
        await _mediator.Send(new ResendConfirmationEmailCommand(request.Email, confirmationBaseUrl));

        return Ok(new { message = "Confirmation email sent if your account exists." });
    }

    [HttpPost("forgotPassword")]
    public async Task<ActionResult> ForgotPassword([FromBody] EmailRequest request)
    {
        var resetBaseUrl = Url.Action("ResetPassword", "Auth", null, Request.Scheme);
        Guard.Against.Null(resetBaseUrl, nameof(resetBaseUrl));
        await _mediator.Send(new ForgotPasswordCommand(request.Email, resetBaseUrl));

        return Ok(new { message = "If your email exists, you'll receive a reset link." });
    }

    [HttpPost("resetPassword")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var success = await _mediator.Send(new ResetPasswordCommand(request));
        if (!success)
            return BadRequest("Password reset failed");

        return Ok(new { message = "Password reset successfully" });
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
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

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
        if (result == null)
            return Unauthorized("Invalid credentials or unconfirmed email");

        return Ok(result);
    }

    /// <summary>
    /// Refresh JWT token
    /// </summary>
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshRequest request)
    {
        var result = await _mediator.Send(new RefreshCommand(request));
        if (result == null)
            return BadRequest("Invalid token");

        return Ok(result);
    }
}

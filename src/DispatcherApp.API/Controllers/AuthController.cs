using System.Net;
using System.Security.Claims;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Model;
using DispatcherApp.Common.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DispatcherApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;



    public AuthController(
        IAuthenticationService authService
        )
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        var confirmationBaseUrl = Url.Action("ConfirmEmail", "Auth", null, Request.Scheme);
        Guard.Against.Null(confirmationBaseUrl, nameof(confirmationBaseUrl));
        var success = await _authService.RegisterAsync(request, confirmationBaseUrl);

        return Ok(new { message = "Confirmation email sent if your account exists." });
    }

    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [HttpGet("confirmEmail")]
    public async Task<ActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            return BadRequest("Invalid confirmation link");

        var success = await _authService.ConfirmEmailAsync(userId, token);
        if (!success)
            return BadRequest("Email confirmation failed");

        return Ok(new { message = "Email confirmed successfully" });
    }

    [HttpPost("resendConfirmationEmail")]
    public async Task<ActionResult> ResendConfirmationEmail([FromBody] EmailRequest request)
    {
        var confirmationBaseUrl = Url.Action("ConfirmEmail", "Auth", null, Request.Scheme);
        Guard.Against.Null(confirmationBaseUrl, nameof(confirmationBaseUrl));
        var success = await _authService.ResendConfirmationEmailAsync(request.Email, confirmationBaseUrl);

        return Ok(new { message = "Confirmation email sent if your account exists." });
    }

    [HttpPost("forgotPassword")]
    public async Task<ActionResult> ForgotPassword([FromBody] EmailRequest request)
    {
        var resetBaseUrl = Url.Action("ResetPassword", "Auth", null, Request.Scheme);
        Guard.Against.Null(resetBaseUrl, nameof(resetBaseUrl));
        var success = await _authService.ForgotPasswordAsync(request.Email, resetBaseUrl);

        return Ok(new { message = "If your email exists, you'll receive a reset link." });
    }

    [HttpPost("resetPassword")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var success = await _authService.ResetPasswordAsync(request);
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

        var userInfo = await _authService.GetUserInfoAsync(userId);
        if (userInfo == null)
            return NotFound();

        return Ok(userInfo);
    }

    [HttpPost("manage/info")]
    [Authorize]
    public async Task<ActionResult> UpdateUserInfo([FromBody] UserInfoResponse request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _authService.UpdateUserInfoAsync(userId, request);
        if (!success)
            return BadRequest("Failed to update user information");

        return Ok(new { message = "User information updated successfully" });
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {

        var result = await _authService.LoginAsync(request);
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
        var result = await _authService.RefreshTokenAsync(request);
        if (result == null)
            return BadRequest("Invalid token");

        return Ok(result);
    }
}

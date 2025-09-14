using System.Security.Claims;
using DispatcherApp.BLL.Interfaces;
using DispatcherApp.Models.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DispatcherApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthenticationService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    //[HttpPost("register")]
    //[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    //public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    //{
    //    var success = await _authService.RegisterAsync(request);
    //    if (!success)
    //        return BadRequest("Registration failed");

    //    return Ok(new { message = "User registered successfully. Please check your email to confirm your account." });
    //}

    //[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    //[HttpGet("confirmEmail")]
    //public async Task<ActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    //{
    //    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
    //        return BadRequest("Invalid confirmation link");

    //    var success = await _authService.ConfirmEmailAsync(userId, token);
    //    if (!success)
    //        return BadRequest("Email confirmation failed");

    //    return Ok(new { message = "Email confirmed successfully" });
    //}

    //[HttpPost("resendConfirmationEmail")]
    //public async Task<ActionResult> ResendConfirmationEmail([FromBody] EmailRequest request)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    await _authService.ResendConfirmationEmailAsync(request.Email);
    //    return Ok(new { message = "If the email exists, a confirmation link has been sent" });
    //}

    //[HttpPost("forgotPassword")]
    //public async Task<ActionResult> ForgotPassword([FromBody] EmailRequest request)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    await _authService.ForgotPasswordAsync(request.Email);
    //    return Ok(new { message = "If the email exists, a reset link has been sent" });
    //}

    //[HttpPost("resetPassword")]
    //public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    var success = await _authService.ResetPasswordAsync(request);
    //    if (!success)
    //        return BadRequest("Password reset failed");

    //    return Ok(new { message = "Password reset successfully" });
    //}

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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(request);
        if (result == null)
            return Unauthorized("Invalid credentials or unconfirmed email");

        return Ok(result);
    }
}

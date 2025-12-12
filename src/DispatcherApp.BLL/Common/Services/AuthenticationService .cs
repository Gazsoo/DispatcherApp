using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.Configurations;
using DispatcherApp.Common.DTOs.Auth;
using DispatcherApp.Common.Exceptions;
using DispatcherApp.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DispatcherApp.BLL.Common.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ITokenService tokenService,
        IEmailSender emailSender,
        IConfiguration configuration,
        IOptions<ApplicationInformation> appInfo,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthenticationService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("Email confirmation attempt for non-existent user: {UserId}", userId);
            throw new InvalidCredentialsException();
        }
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            _logger.LogInformation("Email confirmed for user: {UserId}", userId);
            return true;
        }
        throw new ForbiddenAccessException();
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogWarning("Login attempt for non-existent user: {Email}", request.Email);
            throw new InvalidCredentialsException();
        }

        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            _logger.LogWarning("Login attempt for unconfirmed email: {Email}", request.Email);
            throw new EmailNotConfirmedException();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed login attempt for user: {Email}", request.Email);
            throw new InvalidCredentialsException();
        }

        var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);


        return new AuthResponse
        {
            AccessToken = accessToken.AccessToken,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            ExpiresIn = accessToken.ExpiresIn
        };
    }

    public async Task<AuthResponse?> RefreshTokenAsync(RefreshRequest request)
    {
        try
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                _logger.LogWarning("Invalid access token provided for refresh");
                return null;
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            Guard.Against.NullOrWhiteSpace(userId, message: "userId found.");
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("User not found for token refresh: {UserId}", userId);
                return null;
            }

            if (!await _tokenService.ValidateRefreshTokenAsync(user, request.RefreshToken))
            {
                _logger.LogWarning("Invalid refresh token for user: {UserId}", userId);
                return null;
            }

            var newAccessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(user);

            _logger.LogInformation("Token refreshed for user: {UserId}", userId);

            return new AuthResponse
            {
                AccessToken = newAccessToken.AccessToken,
                RefreshToken = newRefreshToken,
                TokenType = "Bearer",
                ExpiresIn = newAccessToken.ExpiresIn
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return null;
        }
    }

    public async Task<bool> RegisterAsync(RegisterRequest request, string confirmationBaseUrl)
    {

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidCredentialsException();
        }

        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = false
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            throw new ValidationException("User could not be created");
        }

        var roleToAssign = string.IsNullOrWhiteSpace(request.Role) ? Roles.User : request.Role;
        var roleResult = await _userManager.AddToRoleAsync(user, roleToAssign);
        if (!roleResult.Succeeded)
        {
            throw new ValidationException("Role could not be assigned");
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationUrl = $"{confirmationBaseUrl}?userId={user.Id}&token={WebUtility.UrlEncode(token)}";

        await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
            $"Please confirm your email by clicking <a href='{confirmationUrl}'>here</a>");

        return true;
    }

}

using System.Data;
using System.Net;
using System.Security.Claims;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Interfaces;
using DispatcherApp.Models.CommonConfigurations;
using DispatcherApp.Models.DTOs.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DispatcherApp.BLL.Services;

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
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Email confirmation attempt for non-existent user: {UserId}", userId);
                return false;
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                _logger.LogInformation("Email confirmed for user: {UserId}", userId);
                return true;
            }

            _logger.LogWarning("Email confirmation failed for user: {UserId}", userId);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during email confirmation for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> ForgotPasswordAsync(string email, string resetUrl)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogWarning("Password reset attempt for invalid user: {Email}", email);
                return true;
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var finalResetUrl = $"{resetUrl}?email={user.Email}&token={WebUtility.UrlEncode(resetToken)}";

            await _emailSender.SendEmailAsync(email, "Reset your password",
                $"Reset your password by clicking <a href='{finalResetUrl}'>here</a>");

            _logger.LogInformation("Password reset email sent to: {Email}", email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password for: {Email}", email);
            return false;
        }
    }

    public async Task<UserInfoResponse?> GetUserInfoAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Get user info attempt for non-existent user: {UserId}", userId);
                return null;
            }

            return new UserInfoResponse
            {
                Email = user.Email ?? string.Empty,
                EmailConfirmed = user.EmailConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user info for: {UserId}", userId);
            return null;
        }
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt for non-existent user: {Email}", request.Email);
                return null;
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogWarning("Login attempt for unconfirmed email: {Email}", request.Email);
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed login attempt for user: {Email}", request.Email);
                return null;
            }

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);

            _logger.LogInformation("Successful login for user: {Email}", request.Email);

            return new AuthResponse
            {
                AccessToken = accessToken.AccessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresIn = accessToken.ExpiresIn
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Email}", request.Email);
            return null;
        }
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
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration attempt for existing user: {Email}", request.Email);
                return false;
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
                _logger.LogWarning("User creation failed for: {Email}. Errors: {Errors}",
                    request.Email, string.Join(", ", createResult.Errors.Select(e => e.Description)));
                return false;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationUrl = $"{confirmationBaseUrl}?userId={user.Id}&token={WebUtility.UrlEncode(token)}";

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your email by clicking <a href='{confirmationUrl}'>here</a>");

            _logger.LogInformation("User registered successfully: {Email}", request.Email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for user: {Email}", request.Email);
            return false;
        }
    }

    public async Task<bool> ResendConfirmationEmailAsync(string email, string confirmationBaseUrl)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Resend confirmation attempt for non-existent user: {Email}", email);
                return false;
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogInformation("Resend confirmation attempt for already confirmed user: {Email}", email);
                return true;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationUrl = $"{confirmationBaseUrl}?userId={user.Id}&token={WebUtility.UrlEncode(token)}";

            await _emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your email by clicking <a href='{confirmationUrl}'>here</a>");

            _logger.LogInformation("Confirmation email resent to: {Email}", email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during resend confirmation email for: {Email}", email);
            return false;
        }
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset attempt for non-existent user: {Email}", request.Email);
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset successfully for user: {Email}", request.Email);
                return true;
            }

            _logger.LogWarning("Password reset failed for user: {Email}", request.Email);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset for user: {Email}", request.Email);
            return false;
        }
    }

    public async Task<bool> UpdateUserInfoAsync(string userId, UserInfoResponse userInfo)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Update user info attempt for non-existent user: {UserId}", userId);
                return false;
            }

            // Update allowed fields
            if (user.Email != userInfo.Email)
            {
                var changeEmailResult = await _userManager.SetEmailAsync(user, userInfo.Email);
                if (!changeEmailResult.Succeeded)
                {
                    _logger.LogWarning("Failed to update email for user: {UserId}", userId);
                    return false;
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                _logger.LogInformation("User info updated for: {UserId}", userId);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user info for: {UserId}", userId);
            return false;
        }
    }
}

using DispatcherApp.BLL.Interfaces;
using DispatcherApp.Models.CommonConfigurations;
using DispatcherApp.Models.DTOs.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DispatcherApp.BLL.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenService _tokenService;
    //private readonly IEmailSender _emailSender;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly ApplicationInformation _appInfo;

    public AuthenticationService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ITokenService tokenService,
        //IEmailSender emailSender,
        IConfiguration configuration,
        IOptions<ApplicationInformation> appInfo,
        ILogger<AuthenticationService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        //_emailSender = emailSender;
        _logger = logger;
        _appInfo = appInfo.Value;
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

    public Task<bool> ForgotPasswordAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<UserInfoResponse?> GetUserInfoAsync(string userId)
    {
        throw new NotImplementedException();
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
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresIn = 3600
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Email}", request.Email);
            return null;
        }
    }

    public Task<AuthResponse?> RefreshTokenAsync(RefreshRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        //try
        //{
        //    var existingUser = await _userManager.FindByEmailAsync(request.Email);
        //    if (existingUser != null)
        //    {
        //        _logger.LogWarning("Registration attempt for existing user: {Email}", request.Email);
        //        return false;
        //    }

        //    var user = new IdentityUser
        //    {
        //        UserName = request.Email,
        //        Email = request.Email,
        //        EmailConfirmed = false
        //    };

        //    var result = await _userManager.CreateAsync(user, request.Password);
        //    if (!result.Succeeded)
        //    {
        //        _logger.LogWarning("Failed to create user: {Email}, Errors: {Errors}",
        //            request.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
        //        return false;
        //    }

        //    // Send email confirmation
        //    var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    var confirmationUrl = $"{_appInfo.AppUrl}/api/auth/confirmEmail?userId={user.Id}&token={Uri.EscapeDataString(emailToken)}";

        //    await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
        //        $"Please confirm your email by clicking <a href='{confirmationUrl}'>here</a>");

        //    _logger.LogInformation("User registered successfully: {Email}", request.Email);
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "Error during registration for user: {Email}", request.Email);
        //    return false;
        //}
        await Task.CompletedTask;
        return false;
    }

    public Task<bool> ResendConfirmationEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateUserInfoAsync(string userId, UserInfoResponse userInfo)
    {
        throw new NotImplementedException();
    }
}

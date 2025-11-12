using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.Common.DTOs.Auth;
using LoginRequest = DispatcherApp.Common.DTOs.Auth.LoginRequest;
using RefreshRequest = DispatcherApp.Common.DTOs.Auth.RefreshRequest;
using RegisterRequest = DispatcherApp.Common.DTOs.Auth.RegisterRequest;
using ResetPasswordRequest = DispatcherApp.Common.DTOs.Auth.ResetPasswordRequest;

namespace DispatcherApp.BLL.Common.Interfaces;
public interface IAuthenticationService
{
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<AuthResponse?> RefreshTokenAsync(RefreshRequest request);
    Task<bool> RegisterAsync(RegisterRequest request, string confirmationBaseUtl);
    Task<bool> ConfirmEmailAsync(string userId, string token);
    Task<bool> ResendConfirmationEmailAsync(string email, string confirmationBaseUrl);
    Task<bool> ForgotPasswordAsync(string email, string resetUrl);
    Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
}

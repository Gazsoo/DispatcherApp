using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.BLL.Model;
using DispatcherApp.Models.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using LoginRequest = DispatcherApp.Models.DTOs.Auth.LoginRequest;
using RefreshRequest = DispatcherApp.Models.DTOs.Auth.RefreshRequest;
using RegisterRequest = DispatcherApp.Models.DTOs.Auth.RegisterRequest;
using ResetPasswordRequest = DispatcherApp.Models.DTOs.Auth.ResetPasswordRequest;

namespace DispatcherApp.BLL.Interfaces;
public interface IAuthenticationService
{
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<AuthResponse?> RefreshTokenAsync(RefreshRequest request);
    Task<bool> RegisterAsync(RegisterRequest request, string confirmationBaseUtl);
    Task<bool> ConfirmEmailAsync(string userId, string token);
    Task<bool> ResendConfirmationEmailAsync(string email, string confirmationBaseUrl);
    Task<bool> ForgotPasswordAsync(string email, string resetUrl);
    Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
    Task<UserInfoResponse?> GetUserInfoAsync(string userId);
    Task<bool> UpdateUserInfoAsync(string userId, UserInfoResponse userInfo);
}

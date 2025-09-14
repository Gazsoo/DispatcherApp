using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DispatcherApp.BLL.Interfaces;
public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(IdentityUser user);
    Task<string> GenerateRefreshTokenAsync(IdentityUser user);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    Task<bool> ValidateRefreshTokenAsync(IdentityUser user, string refreshToken);
}

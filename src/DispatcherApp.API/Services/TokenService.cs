using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Configurations;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DispatcherApp.API.Services;
public class TokenService : ITokenService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly TimeProvider _timeProvider;

    public  TokenService(
        UserManager<IdentityUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        TimeProvider timeProvider
        )
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
        _timeProvider = timeProvider;
    }
    public async Task<TokenResult>  GenerateAccessTokenAsync(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var now = _timeProvider.GetUtcNow();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var expiresAt = now.AddMinutes(_jwtSettings.ExpiryMinutes).UtcDateTime;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new TokenResult
        {
            AccessToken = tokenHandler.WriteToken(token),
            ExpiresAt = expiresAt,
            TimeProvider = _timeProvider
        };
    }

    public async Task<string> GenerateRefreshTokenAsync(IdentityUser user)
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);

        await _userManager.UpdateSecurityStampAsync(user);

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(IdentityUser user, string refreshToken)
    {
        await Task.CompletedTask;
        return !string.IsNullOrEmpty(refreshToken) && refreshToken.Length > 0;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
            ValidateLifetime = false // we are checking expired tokens here
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

}

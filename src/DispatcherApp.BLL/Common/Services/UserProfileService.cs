using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.Abstractions.Repository;
using DispatcherApp.Common.DTOs.User;
using DispatcherApp.Common.Exceptions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DispatcherApp.BLL.Common.Services;

public sealed class UserProfileService : IUserProfileService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<UserProfileService> _logger;
    private readonly IUserRepository _userRepository;

    public UserProfileService(
        UserManager<IdentityUser> userManager, 
        ILogger<UserProfileService> logger,
        IUserRepository repository
        )
    {
        _userRepository = repository;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IReadOnlyList<UserInfoResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var users = await _userManager.Users.ToListAsync(ct);
        var results = new List<UserInfoResponse>(users.Count);
        foreach (var user in users)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            results.Add(BuildResponse(user, claims, roles));
        }

        return results;
    }

    public async Task<UserInfoResponse> GetAsync(string userId, CancellationToken ct = default)
    {

        var user = await _userManager.FindByIdAsync(userId);
        Guard.Against.NotFound(userId, user);

        var claims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        return BuildResponse(user, claims, roles);

    }

    public async Task<UserInfoResponse> UpdateAsync(string userId, UserInfoResponse profile, CancellationToken ct = default)
    {

        var user = await _userManager.FindByIdAsync(profile.Id);
        if (user == null)
        {
            _logger.LogWarning("Profile update for non-existent user {UserId}", userId);
        }
        Guard.Against.NotFound(userId, user);

        if (!string.Equals(user.Email, profile.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailResult = await _userManager.SetEmailAsync(user, profile.Email);
            if (!emailResult.Succeeded)
            {
                _logger.LogWarning("Failed to update email for {UserId}: {Errors}", userId,
                    string.Join(", ", emailResult.Errors.Select(e => e.Description)));
                throw CreateIdentityValidationException("Email", emailResult.Errors);
            }
        }

        user.PhoneNumber = profile.Phone;
        user.TwoFactorEnabled = profile.TwoFactorEnabled;

        var claims = await _userManager.GetClaimsAsync(user);
        await UpsertClaimAsync(user, claims, ClaimTypes.GivenName, profile.FirstName);
        await UpsertClaimAsync(user, claims, ClaimTypes.Surname, profile.LastName);

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            throw CreateIdentityValidationException("Profile", updateResult.Errors);
        }
        await _userRepository.SetRoleAsync(user, profile.Role, ct);
        var roles = await _userManager.GetRolesAsync(user);
        return BuildResponse(user, claims, roles);
    }

    private static UserInfoResponse BuildResponse(
        IdentityUser user,
        IEnumerable<Claim> claims,
        IEnumerable<string> roles)
    {
        return new UserInfoResponse
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            EmailConfirmed = user.EmailConfirmed,
            Phone = user.PhoneNumber,
            TwoFactorEnabled = user.TwoFactorEnabled,
            FirstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? string.Empty,
            LastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? string.Empty,
            Role = roles.FirstOrDefault() ?? string.Empty
        };
    }

    private async Task UpsertClaimAsync(
        IdentityUser user,
        IList<Claim> claims,
        string claimType,
        string? newValue)
    {
        var existing = claims.FirstOrDefault(c => c.Type == claimType);

        if (string.IsNullOrWhiteSpace(newValue))
        {
            if (existing != null)
            {
                await _userManager.RemoveClaimAsync(user, existing);
                claims.Remove(existing);
            }

            return;
        }

        var updatedClaim = new Claim(claimType, newValue);
        if (existing == null)
        {
            await _userManager.AddClaimAsync(user, updatedClaim);
            claims.Add(updatedClaim);
            return;
        }

        if (!string.Equals(existing.Value, newValue, StringComparison.Ordinal))
        {
            await _userManager.ReplaceClaimAsync(user, existing, updatedClaim);
            claims.Remove(existing);
            claims.Add(updatedClaim);
        }
    }

    private static ValidationException CreateIdentityValidationException(string field, IEnumerable<IdentityError> errors)
    {
        var failures = errors.Select(e => new ValidationFailure(field, e.Description));
        return new ValidationException(failures);
    }
}

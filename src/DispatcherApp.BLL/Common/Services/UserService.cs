using System.Data;
using System.Net;
using System.Security.Claims;
using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.Abstractions.Repository;
using DispatcherApp.Common.Configurations;
using DispatcherApp.Common.DTOs.Auth;
using DispatcherApp.Common.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DispatcherApp.BLL.Common.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UserService> _logger;

    public UserService(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        IUserRepository repository,
        ILogger<UserService> logger)

    {
        _repository = repository;
        _roleManager = roleManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserInfoResponse> UpdateAsync(UserInfoUpdateRequest request, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(request.UserId, ct);

        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            await _repository.UpdateEmailAsync(user, request.Email, ct);
        }
        await _repository.UpdateAsync(user, ct);

        return _mapper.Map<UserInfoResponse>(user);
    }

    public async Task<List<UserInfoResponse>> GetAllAsync(CancellationToken ct)
    {
        var users = await _repository.GetAllAsync(ct);
        return _mapper.Map<List<UserInfoResponse>>(users);
    }

    public async Task<string> GetUserRoleByIdAsync(string id, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        var role = await _repository.GetRoleAsync(user, ct);
        return role.FirstOrDefault() ?? string.Empty;

    }
    public async Task<UserInfoResponse> GetByIdAsync(string id, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        return _mapper.Map<UserInfoResponse>(user);
    }

    public async Task<UserInfoResponse> UpdateRoleAsync(string userId, string role, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(userId, ct);
        await _repository.SetRoleAsync(user, role, ct);
        return _mapper.Map<UserInfoResponse>(user);
    }

    public async Task<UserInfoResponse> CreateAsync(CreateUserRequest request, CancellationToken ct)
    {
        var user = await _repository.CreateAsync(new IdentityUser
        {   EmailConfirmed = true,
            UserName = request.Email,
            Email = request.Email
        }, request.Password, ct);
        return await UpdateRoleAsync(user.Id, request.Role, ct);
    }

    public async Task DeleteAsync(string userId, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(userId, ct);
        Guard.Against.NotFound(userId, nameof(user));
        await _repository.DeleteAsync(user, ct);
    }
}

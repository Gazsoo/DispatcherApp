using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using DispatcherApp.Common.Abstractions.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DispatcherApp.DAL.Repositories;
public class UserRepository: IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<IdentityUser>> GetAllAsync(CancellationToken ct)
        => await _userManager.Users.ToListAsync(ct);

    public async Task<IdentityUser> GetByIdAsync(string id, CancellationToken ct)
        => await _userManager.FindByIdAsync(id)
            ?? throw new NotFoundException(id, nameof(id));

    public async Task<IdentityUser> CreateAsync(IdentityUser user, string password, CancellationToken ct)
    {
        await _userManager.CreateAsync(user, password);
        return user;
    }
    public async Task UpdateEmailAsync(IdentityUser user, string newEmail, CancellationToken ct)
    {
        var result = await _userManager.SetEmailAsync(user, newEmail);
        if (!result.Succeeded)
            throw new NotFoundException(user.Id, nameof(user));
    }

    public async Task UpdateAsync(IdentityUser user, CancellationToken ct)
        => await _userManager.UpdateAsync(user);

    public async Task SetRoleAsync(IdentityUser user, string role, CancellationToken ct)
    {
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Any())
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
            throw new NotFoundException(role, (nameof(user)));
    }

    public async Task DeleteAsync(IdentityUser user, CancellationToken ct)
    {
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new NotFoundException(user.Id, (nameof(user)));
    }

    public async Task<IEnumerable<string>> GetRoleAsync(IdentityUser user, CancellationToken ct)
    {
        return await _userManager.GetRolesAsync(user);
    }
}

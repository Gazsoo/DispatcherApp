using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.DAL.Data;
using DispatcherApp.Models.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DispatcherApp.BLL.Services;

public static class RoleSeederExtention
{
    public static async Task<WebApplication> RoleSeed(this WebApplication serviceProvider)
    {
        using var scope = serviceProvider.Services.CreateScope();
        var initialiser = scope.ServiceProvider.GetRequiredService<DatabaseSeedingService>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();

        return serviceProvider;
    }
}

public class DatabaseSeedingService
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<DatabaseSeedingService> _logger;

    public DatabaseSeedingService(
        AppDbContext context,
        ILogger<DatabaseSeedingService> logger,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
    public async Task TrySeedAsync()
    {
        // Admin
        var administratorRole = new IdentityRole(Roles.Administrator);
        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Other roles
        foreach (string role in Roles.All)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        
        // Default users
        var administrator = new IdentityUser { 
            UserName = "administrator@localhost", 
            Email = "administrator@localhost",
            EmailConfirmed = true
            };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

    }
}

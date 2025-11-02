using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.DAL.Data;
using DispatcherApp.Common.Constants;
using DispatcherApp.Common.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using File = DispatcherApp.Common.Entities.File;

namespace DispatcherApp.DAL.Seed;

public static class RoleSeederExtention
{
    public static async Task<WebApplication> RoleSeed(this WebApplication serviceProvider)
    {
        using var scope = serviceProvider.Services.CreateScope();
        var initialiser = scope.ServiceProvider.GetRequiredService<DatabaseSeedingService>();

        //await initialiser.InitialiseAsync();
        //await initialiser.SeedAsync();
        await Task.CompletedTask;
        return serviceProvider;
    }
}

public class DatabaseSeedingService
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<DatabaseSeedingService> _logger;
    private readonly TimeProvider _timeProvider;

    public DatabaseSeedingService(
        AppDbContext context,
        ILogger<DatabaseSeedingService> logger,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        TimeProvider timeProvider
        )
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _timeProvider = timeProvider;
    }
    public async Task InitialiseAsync()
    {
        try
        {
            await Task.CompletedTask;
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.MigrateAsync();
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
        administratorRole.Id = "c90ee222-ccd2-4857-8888-161752decd99";
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
        var now = _timeProvider.GetUtcNow().UtcDateTime;
        var tutorial = new Tutorial { 
            CreatedAt = now,
            Description = "This is a sample tutorial",
            Title = "Sample Tutorial",
            Url = "https://example.com/sample-tutorial",
            UpdatedAt = now,
            Files = new List<File>
            {
                new File
                {
                    FileName = "sample.txt",
                    ContentType = "text/plain",
                    FileSize = 1024,
                    OriginalFileName = "sample_original.txt",
                    StoragePath = "files/sample.txt",
                    UploadedAt = now,
                }
            }
        };
        _context.Tutorials.Add(tutorial);
        await _context.SaveChangesAsync();

        var assignment1 = new Assignment
        {
            Name = "Task1",
        };
        var assignment2 = new Assignment
        {
            Name = "Task2",
        };
        _context.Assignments.Add(assignment1);
        _context.Assignments.Add(assignment2);

        var assignmentUser1 = new AssignmentUser
        {
            Assignment = assignment1,
            UserId = administrator.Id,
            AssignedAt = now
        };

        _context.AssignmentUsers.Add(assignmentUser1);
        await _context.SaveChangesAsync();

    }
}

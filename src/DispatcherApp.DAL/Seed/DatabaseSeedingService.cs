using System.Text;
using DispatcherApp.Common.Constants;
using DispatcherApp.Common.Entities;
using DispatcherApp.DAL.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using File = DispatcherApp.Common.Entities.File;

namespace DispatcherApp.DAL.Seed;

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
        int maxRetries = 5;

        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                await Task.CompletedTask;
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.MigrateAsync();
            }
            catch (SqlException ex)
            {
                // Log warning and wait, but DO NOT THROW yet.
                _logger.LogWarning($"Database not ready yet (Error {ex.Number}). Retrying in 3s... (Attempt {i + 1}/{maxRetries})");
                await Task.Delay(3000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
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
        if (_context.Tutorials.Any() && _userManager.Users.Any())
        {
            // DB seeded
            return;
        }
        // Admin
        var administratorRole = new IdentityRole(Roles.Administrator);
        administratorRole.Id = "c90ee222-ccd2-4857-8888-161752decd99";
        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // User
        var userRole = new IdentityRole(Roles.User);
        userRole.Id = "913d93ba-4195-4b96-b34e-e33b41e7aec4";
        if (_roleManager.Roles.All(r => r.Name != userRole.Name))
        {
            await _roleManager.CreateAsync(userRole);
        }

        // Dispatcher
        var dispatcherRole = new IdentityRole(Roles.Dispatcher);
        dispatcherRole.Id = "dd051e4a-2296-4602-b785-82500e438b36";
        if (_roleManager.Roles.All(r => r.Name != dispatcherRole.Name))
        {
            await _roleManager.CreateAsync(dispatcherRole);
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
        var user = new IdentityUser
        {
            UserName = "user@localhost",
            Email = "user@localhost",
            EmailConfirmed = true
        };
        if (_userManager.Users.All(u => u.UserName != user.UserName))
        {
            await _userManager.CreateAsync(user, "User1!");
            if (!string.IsNullOrWhiteSpace(userRole.Name))
            {
                await _userManager.AddToRolesAsync(user, new[] { userRole.Name });
            }
        }
        var dispatcher = new IdentityUser
        {
            UserName = "dispatcher@localhost",
            Email = "dispatcher@localhost",
            EmailConfirmed = true
        };
        if (_userManager.Users.All(u => u.UserName != dispatcher.UserName))
        {
            await _userManager.CreateAsync(dispatcher, "Dispatcher1!");
            if (!string.IsNullOrWhiteSpace(dispatcherRole.Name))
            {
                await _userManager.AddToRolesAsync(dispatcher, new[] { dispatcherRole.Name });
            }
        }

        var now = _timeProvider.GetUtcNow().UtcDateTime;
        var file = new File
        {
            FileName = "sample.txt",
            ContentType = "text/plain",
            FileSize = 1024,
            OriginalFileName = "sample_original.txt",
            StoragePath = "files/sample_original.txt",
            UploadedAt = now,
        };
        
        var tutorial = new Tutorial { 
            CreatedAt = now,
            Description = "This is a sample tutorial",
            Title = "Sample Tutorial",
            Url = "",
            UpdatedAt = now,
            Files = new List<File>
            {
                file
            }
        };
        using (FileStream fs = System.IO.File.Create("wwwroot/uploads/files/sample_original.txt"))
        {
            byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
            // Add some information to the file.
            fs.Write(info, 0, info.Length);
        }
        _context.Tutorials.Add(tutorial);
        await _context.SaveChangesAsync();

        var assignment1 = new Assignment
        {
            Name = "Sample Task1",
            Description = "Sample Description 1",
            PlannedTime = now,
        };
        var assignment2 = new Assignment
        {
            Name = "Sample Task2",
            Description = "Sample Description 2",
            PlannedTime = now,
        };
        _context.Assignments.Add(assignment1);
        _context.Assignments.Add(assignment2);

        var assignmentUser1 = new AssignmentUser
        {
            Assignment = assignment1,
            UserId = administrator.Id,
            AssignedAt = now
        };
        var assignmentUser2 = new AssignmentUser
        {
            Assignment = assignment2,
            UserId = user.Id,
            AssignedAt = now
        };
        var assignmentUser3 = new AssignmentUser
        {
            Assignment = assignment2,
            UserId = dispatcher.Id,
            AssignedAt = now
        };

        _context.AssignmentUsers.Add(assignmentUser1);
        _context.AssignmentUsers.Add(assignmentUser2);
        _context.AssignmentUsers.Add(assignmentUser3);


        _context.DispatcherSessions.Add(new DispatcherSession
        {
            GroupId = "0b60f76b-1c0b-4e95-99b1-dc0acd73c77a",
            LogFile = file,
            OwnerId = dispatcher.Id
        });
        await _context.SaveChangesAsync();

    }
}

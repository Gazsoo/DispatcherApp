using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.BLL.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DispatcherApp.BLL.Services;

//internal static class RoleSeederExtention
//{
//    public static async Task<WebApplication> RoleSeed(this WebApplication serviceProvider)
//    {
//        using var scope = serviceProvider.Services.CreateScope();
//        var initialiser = scope.ServiceProvider.GetRequiredService<DatabaseSeedingService>();

//        await initialiser.InitialiseAsync();
//        await initialiser.SeedAsync();

//        string[] roles = { "Admin", "Dispatcher", "User" };

//        foreach (string role in roles)
//        {
//            if (!await roleManager.RoleExistsAsync(role))
//            {
//                await roleManager.CreateAsync(new IdentityRole(role));
//            }
//        }

//        return serviceProvider;
//    }

//}
//internal class DatabaseSeedingService
//{
//    private readonly AppDbContext _context;
//    //private readonly UserManager<ApplicationUser> _userManager;
//    private readonly RoleManager<IdentityRole> _roleManager;
//    public async Task InitialiseAsync()
//    {
//        try
//        {
//            // See https://jasontaylor.dev/ef-core-database-initialisation-strategies
//            await _context.Database.EnsureDeletedAsync();
//            await _context.Database.EnsureCreatedAsync();
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "An error occurred while initialising the database.");
//            throw;
//        }
//    }
//}

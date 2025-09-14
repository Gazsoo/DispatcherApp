
using System.Text;
using Ardalis.GuardClauses;
using DispatcherApp.DAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDataAccessServices(this IHostApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DispatcherDb");
            Guard.Against.Null(connectionString, message: "Connection string 'DispatcherDb' not found.");


            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>();

            //builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            //builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            builder.Services.AddSingleton(TimeProvider.System);
            builder.Services.AddDbContext<AppDbContext>((sp, options) =>
            {
                //options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
                options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            });
            // Add Identity
            builder.Services.AddIdentityCore<IdentityUser> (options =>
            {
                // Configure identity options
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

            //builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            //builder.Services.AddScoped<ApplicationDbContextInitialiser>();

            //builder.Services
            //    .AddDefaultIdentity<ApplicationUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            //builder.Services.AddSingleton(TimeProvider.System);
            //builder.Services.AddTransient<IIdentityService, IdentityService>();

            //builder.Services.AddAuthorization(options =>
            //    options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));
        }
    }
}

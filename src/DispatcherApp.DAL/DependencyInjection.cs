
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using DispatcherApp.DAL.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDataAccessServices(this IHostApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DispatcherDb");
            Guard.Against.Null(connectionString, message: "Connection string 'DispatcherDb' not found.");

            //builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            //builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
            builder.Services.AddSingleton(TimeProvider.System);
            builder.Services.AddDbContext<AppDbContext>((sp, options) =>
            {
                //options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
                options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            });


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

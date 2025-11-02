
using System.Text;
using Ardalis.GuardClauses;
using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using DispatcherApp.Common.Abstractions;
using DispatcherApp.Common.Abstractions.Repository;
using DispatcherApp.Common.Abstractions.Storage;
using DispatcherApp.DAL.Configurations;
using DispatcherApp.DAL.Data;
using DispatcherApp.DAL.Repositories;
using DispatcherApp.DAL.Seed;
using DispatcherApp.DAL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDataAccessServices(this IHostApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DispatcherDb");
            Guard.Against.Null(connectionString, message: "Connection string 'DispatcherDb' not found.");

            builder.Services.Configure<FileStorageSettings>(builder.Configuration.GetSection(FileStorageSettings.SectionName));
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(EmailSettings.SectionName));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>();

            //builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            //builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            //builder.Services.AddSingleton(TimeProvider.System);
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

            builder.Services.AddScoped<DatabaseSeedingService>();
            //builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
            builder.Services.AddScoped<IFileStorageService, AzureBlobStorageService>();
            builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            builder.Services.AddScoped<IFileRepository, FileRepository>();
            builder.Services.AddScoped<ITutorialRepository, TutorialRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            //builder.Services
            //    .AddDefaultIdentity<ApplicationUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddSingleton(TimeProvider.System);
            //builder.Services.AddTransient<IIdentityService, IdentityService>();

            //builder.Services.AddAuthorization(options =>
            //    options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));


            var credential = new DefaultAzureCredential();
            builder.Services.AddSingleton<TokenCredential>(credential);
            // Register Azure clients
            builder.Services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.UseCredential(credential);

                // Get the blob URI from configuration
                var blobUri = builder.Configuration[$"{FileStorageSettings.SectionName}:BlobUri"];
                if (!string.IsNullOrWhiteSpace(blobUri))
                {
                    clientBuilder.AddBlobServiceClient(new Uri(blobUri));
                }
                // Email
                var emailEndpoint = builder.Configuration["EmailSettings:ConnectionString"];
                if (!string.IsNullOrWhiteSpace(emailEndpoint))
                {
                    clientBuilder.AddEmailClient(emailEndpoint);
                    builder.Services.AddSingleton<IEmailSender, AzureEmailService>();
                    builder.Services.AddSingleton<IEmailSender<IdentityUser>, AzureEmailService>();
                } else
                {
                    builder.Services.AddSingleton<IEmailSender<IdentityUser>, DummyEmailSender>();
                    builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();
                }
            });
        }
    }
}

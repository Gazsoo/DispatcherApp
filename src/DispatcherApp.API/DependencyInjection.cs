using System.Collections.Generic;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using DispatcherApp.DAL.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddApiServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>();

            builder.Services.AddControllers();

            //builder.Services.AddExceptionHandler<CustomExceptionHandler>();

            // Customise default API behaviour
            builder.Services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddOpenApiDocument((configure, sp) =>
            {
                configure.Title = "DispatcherApp API";
            });
        }

        public static void AddKeyVaultIfConfigured(this IHostApplicationBuilder builder)
        {
            //var keyVaultUri = builder.Configuration["AZURE_KEY_VAULT_ENDPOINT"];
            //if (!string.IsNullOrWhiteSpace(keyVaultUri))
            //{
            //    builder.Configuration.AddAzureKeyVault(
            //        new Uri(keyVaultUri),
            //        new DefaultAzureCredential());
            //}
        }
    }
}

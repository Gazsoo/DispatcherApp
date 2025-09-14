using DispatcherApp.API.Configurations;
using DispatcherApp.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static void AddApiServices(this IHostApplicationBuilder builder)
        {
            builder.Configuration.AddApiConfigurations();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();

            //builder.Services.AddExceptionHandler<CustomExceptionHandler>();

            // Customise default API behaviour
            builder.Services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true);

            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddIdentityApiEndpoints<IdentityUser>();
            builder.Services.AddOpenApiDocument((configure, sp) =>
            {
                configure.Title = "DispatcherApp API";
                configure.AddSecurity("Bearer", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Name = "Authorization",
                    Description = "Enter JWT Bearer token"
                });
                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));

            });
            //builder.Services.AddSingleton<IEmailSender<IdentityUser>, DummyEmailSender>();
            builder.Services.AddSingleton<IEmailSender<IdentityUser>, DummyEmailSender>();
            //builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();
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


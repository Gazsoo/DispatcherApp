
using DispatcherApp.API.Services;
using DispatcherApp.BLL.Configurations;
using DispatcherApp.BLL.Extentions;
using DispatcherApp.BLL.Interfaces;
using DispatcherApp.BLL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddBusinessLogicServices(this IHostApplicationBuilder builder)
        {
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
            builder.Services.Configure<FileStorageSettings>(builder.Configuration.GetSection(FileStorageSettings.SectionName));

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = null;
                options.LogoutPath = null;
                options.AccessDeniedPath = null;
            });
            

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddSingleton<IEmailSender<IdentityUser>, DummyEmailSender>();
            builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();
            builder.Services.AddScoped<IUserContextService, UserContextService>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();
            builder.Services.AddScoped<ITutorialService, TutorialService>();
            builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();

        }

    }

}

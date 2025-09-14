
using DispatcherApp.BLL.Services;
using DispatcherApp.BLL.Configurations;
using DispatcherApp.BLL.Extentions;
using DispatcherApp.BLL.Interfaces;
using Microsoft.Extensions.Hosting;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddBusinessLogicServices(this IHostApplicationBuilder builder)
        {
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = null;
                options.LogoutPath = null;
                options.AccessDeniedPath = null;
            });

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

        }
    }
}

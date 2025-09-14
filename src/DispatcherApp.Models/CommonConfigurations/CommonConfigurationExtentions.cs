
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;


namespace DispatcherApp.Models.CommonConfigurations;
public static class CommonConfigurationExtentions
{

    public static void AddCommonConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<ApplicationInformation>(builder.Configuration.GetSection(ApplicationInformation.SectionName));
    }
  
}

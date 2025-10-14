namespace DispatcherApp.API.Configurations;

public class CorsSettings
{
    public const string SectionName = "CorsSettings";
    public string PolicyName { get; set; } = "DefaultPolicy";
    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
}

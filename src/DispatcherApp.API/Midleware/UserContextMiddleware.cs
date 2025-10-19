using System.Security.Claims;
using DispatcherApp.Common.Context;

namespace DispatcherApp.API.Midleware;

public class UserContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserContextMiddleware> _logger;
    public const string UserContextKey = nameof(UserContext);

    public UserContextMiddleware(
        RequestDelegate next,
        ILogger<UserContextMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            try
            {
                var userContext = ExtractUserContext(context.User);
                context.Items[UserContextKey] = userContext;

                _logger.LogDebug("User context set for {UserId}", userContext.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to extract user context");
            }
        }

        await _next(context);
    }
    private UserContext ExtractUserContext(ClaimsPrincipal principal)
    {
        return new UserContext
        {
            UserId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? principal.FindFirst("sub")?.Value,
            UserName = principal.FindFirst(ClaimTypes.Name)?.Value
                      ?? principal.FindFirst("name")?.Value,
            Email = principal.FindFirst(ClaimTypes.Email)?.Value
                   ?? principal.FindFirst("email")?.Value,
            Roles = principal.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value)
                .Distinct()
                .ToList(),
            Claims = principal.Claims
                .GroupBy(c => c.Type)
                .ToDictionary(g => g.Key, g => g.First().Value),
            TenantId = principal.FindFirst("tenant_id")?.Value
        };
    }
}

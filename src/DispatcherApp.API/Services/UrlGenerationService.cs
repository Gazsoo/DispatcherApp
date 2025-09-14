using Ardalis.GuardClauses;

namespace DispatcherApp.API.Services;

public class UrlGenerationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LinkGenerator _linkGenerator;

    public UrlGenerationService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    {
        _httpContextAccessor = httpContextAccessor;
        _linkGenerator = linkGenerator;
    }

    public string GenerateConfirmationUrl(string userId, string token)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        Guard.Against.Null(httpContext, nameof(httpContext));

        var uri = _linkGenerator.GetUriByAction(
            httpContext,
            "ConfirmEmail",
            "Auth",
            new { userId, token });
        Guard.Against.NullOrEmpty(uri, nameof(uri));

        return uri;
    }
    public string GeneratePasswordResetUrl(string email, string token) {
        var httpContext = _httpContextAccessor.HttpContext;
        Guard.Against.Null(httpContext, nameof(httpContext));

        var uri = _linkGenerator.GetUriByAction(
            httpContext,
            "ResetPassword",
            "Auth",
            new{ email, token });
        Guard.Against.NullOrEmpty(uri, nameof(uri));

        return uri;
    }
}

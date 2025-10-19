using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.Context;
using Microsoft.AspNetCore.Http;

namespace DispatcherApp.API.Services;
public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private UserContext? _cachedContext;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserContext? GetCurrentUser()
    {
        if (_cachedContext != null)
            return _cachedContext;

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.Items.TryGetValue("UserContext", out var contextObj) == true
            && contextObj is UserContext userContext)
        {
            _cachedContext = userContext;
            return userContext;
        }

         return null;
    }

    public string? UserId => GetCurrentUser()?.UserId;
    public string? UserName => GetCurrentUser()?.UserName;
    public string? Email => GetCurrentUser()?.Email;
    public bool IsAuthenticated => GetCurrentUser() != null;

    public bool IsInRole(string role) =>
        GetCurrentUser()?.IsInRole(role) ?? false;

    public bool HasClaim(string claimType, string claimValue) =>
        GetCurrentUser()?.Claims?.TryGetValue(claimType, out var value) == true
        && value == claimValue;
}

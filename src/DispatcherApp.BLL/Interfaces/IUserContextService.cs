using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.Models.Context;

namespace DispatcherApp.BLL.Interfaces;
public interface IUserContextService
{
    UserContext? GetCurrentUser();
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
    bool HasClaim(string claimType, string claimValue);
}

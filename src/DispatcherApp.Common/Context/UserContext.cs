using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.Context;
public class UserContext
{
    public required string? UserId { get; init; }
    public required string? UserName { get; init; }
    public required string? Email { get; init; }
    public List<string> Roles { get; set; } = new();
    public Dictionary<string, string> Claims { get; set; } = new();
    public string? TenantId { get; set; }

    public bool IsInRole(string role) => Roles?.Contains(role) ?? false;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.DTOs.User;
public record UserInfoResponse
{
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool TwoFactorEnabled { get; set; }
    public required string  Id { get; set; }
    public string? Phone { get; set; }
}

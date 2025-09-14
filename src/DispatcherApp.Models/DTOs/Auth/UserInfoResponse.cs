using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Models.DTOs.Auth;
public record UserInfoResponse
{
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool TwoFactorEnabled { get; set; }
}

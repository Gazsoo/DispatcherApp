using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DispatcherApp.BLL.Model;
public class RegisterResult
{
    public IdentityUser? User { get; set; }
    public  string EmailConfirmationToken { get; set; } = string.Empty;
    public bool Success { get; set; }
}

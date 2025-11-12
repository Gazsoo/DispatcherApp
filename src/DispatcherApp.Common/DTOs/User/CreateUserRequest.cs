using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatcherApp.Common.DTOs.User;
public record CreateUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Role,
    string Password
    )
{
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DispatcherApp.Common.Abstractions.Repository;
public interface IUserRepository
{
    Task<IdentityUser> GetByIdAsync(string id, CancellationToken ct);
    Task<List<IdentityUser>> GetAllAsync(CancellationToken ct);
    Task<IdentityUser> CreateAsync(IdentityUser user, string password, CancellationToken ct);
    Task UpdateAsync(IdentityUser user, CancellationToken ct);
    Task UpdateEmailAsync(IdentityUser user, string newEmail, CancellationToken ct);
    Task<IEnumerable<string>> GetRoleAsync(IdentityUser user, CancellationToken ct);
    Task SetRoleAsync(IdentityUser user, string role, CancellationToken ct);
    Task DeleteAsync(IdentityUser user, CancellationToken ct);
}

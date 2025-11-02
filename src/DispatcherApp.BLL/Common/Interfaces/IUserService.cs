using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.Common.DTOs.User;

namespace DispatcherApp.BLL.Common.Interfaces;

public interface IUserService
{
    Task<List<UserInfoResponse>> GetAllAsync(CancellationToken ct);
    Task<UserInfoResponse> GetByIdAsync(string id, CancellationToken ct);
    Task<UserInfoResponse> CreateAsync(CreateUserRequest request, CancellationToken ct);
    Task DeleteAsync(string userId, CancellationToken ct);
    Task<UserInfoResponse> UpdateRoleAsync(string userId, string role, CancellationToken ct);
    Task<UserInfoResponse> UpdateAsync(UserInfoUpdateRequest request, CancellationToken ct);
}

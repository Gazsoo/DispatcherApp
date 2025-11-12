using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.Common.DTOs.User;

namespace DispatcherApp.BLL.Common.Interfaces;

public interface IUserProfileService
{
    Task<UserInfoResponse> GetAsync(string userId, CancellationToken ct = default);
    Task<IReadOnlyList<UserInfoResponse>> GetAllAsync(CancellationToken ct = default);
    Task<UserInfoResponse> UpdateAsync(string userId, UserInfoResponse profile, CancellationToken ct = default);
}

using DispatcherApp.Common.Context;
using DispatcherApp.Common.DTOs.Assignment;

namespace DispatcherApp.BLL.Common.Interfaces;
public interface IAssignmentService
{
    UserContext? GetCurrentUserId();
    Task<IEnumerable<AssignmentResponse>> GetAssignmentListAsync();
    Task<AssignmentWithUsersResponse> GetAssignmentAsync(int id);
    Task<IEnumerable<AssignmentResponse>> GetUserAssignmentAsync();
}

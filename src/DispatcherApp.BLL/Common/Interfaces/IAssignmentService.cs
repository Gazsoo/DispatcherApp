using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DispatcherApp.Common.Context;
using DispatcherApp.Common.DTOs.Assignment;
using DispatcherApp.Common.Entities;

namespace DispatcherApp.BLL.Common.Interfaces;

public interface IAssignmentService
{
    Task<IEnumerable<AssignmentWithUsersResponse>> GetAssignmentListAsync(CancellationToken ct = default);
    Task<AssignmentWithUsersResponse> GetAssignmentAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<AssignmentResponse>> GetUserAssignmentAsync(CancellationToken ct = default);
    Task<AssignmentWithUsersResponse> CreateAssignmentAsync(AssignmentCreateRequest request, CancellationToken ct);
    Task<AssignmentWithUsersResponse> UpdateAssignmentAsync(int assignmentId, AssignmentUpdateRequest request, CancellationToken ct);
    Task<AssignmentWithUsersResponse> UpdateAssignmentStatusAsync(int assignmentId, AssignmentStatus status, CancellationToken ct);
    Task<AssignmentWithUsersResponse> AddAssigneesAsync(int assignmentId, IEnumerable<string> userIds, CancellationToken ct);
    Task<AssignmentWithUsersResponse> RemoveAssigneeAsync(int assignmentId, string userId, CancellationToken ct);
    Task DeleteAssignmentAsync(int assignmentId, CancellationToken ct);
}

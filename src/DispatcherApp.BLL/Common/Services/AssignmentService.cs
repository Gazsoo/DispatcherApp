using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.Abstractions.Repository;
using DispatcherApp.Common.Context;
using DispatcherApp.Common.DTOs.Assignment;
using DispatcherApp.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DispatcherApp.BLL.Common.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IUserContextService _userContextService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAssignmentRepository _repository;
    private readonly IMapper _mapper;
    private readonly TimeProvider _timeProvider;

    public AssignmentService(
        IUserContextService userContextService,
        UserManager<IdentityUser> userManager,
        IMapper mapper,
        IAssignmentRepository repository,
        TimeProvider timeProvider)
    {
        _userContextService = userContextService;
        _userManager = userManager;
        _mapper = mapper;
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public async Task<IEnumerable<AssignmentResponse>> GetAssignmentListAsync(CancellationToken ct = default)
    {
        var assignments = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<AssignmentResponse>>(assignments);
    }

    public async Task<AssignmentWithUsersResponse> GetAssignmentAsync(int id, CancellationToken ct = default)
    {
        var assignment = await _repository.GetByIdAsync(id);
        Guard.Against.NotFound(id, assignment);

        return await MapWithUsersAsync(assignment, ct);
    }

    public async Task<IEnumerable<AssignmentResponse>> GetUserAssignmentAsync(CancellationToken ct = default)
    {
        var user = _userContextService.GetCurrentUser();
        var userId = Guard.Against.NullOrEmpty(user?.UserId, nameof(user.UserId));
        var assignments = await _repository.GetUserAssignments(userId);
        return _mapper.Map<IEnumerable<AssignmentResponse>>(assignments);
    }

    public async Task<AssignmentWithUsersResponse> CreateAssignmentAsync(AssignmentCreateRequest request, CancellationToken ct)
    {
        Guard.Against.Null(request, nameof(request));

        var assignment = new Assignment
        {
            Name = request.Name,
            Description = request.Description ?? string.Empty,
            PlannedTime = request.PlannedTime,
            Status = request.Status,
            Type = request.Type ?? string.Empty,
            Value = request.Value ?? string.Empty,
            CreatedAt = _timeProvider.GetUtcNow().UtcDateTime
        };

        await AddAssigneesToAssignmentAsync(assignment, request.AssigneeIds ?? Array.Empty<string>(), ct);

        await _repository.AddAsync(assignment);
        await _repository.SaveChangesAsync();

        return await MapWithUsersAsync(assignment, ct);
    }

    public async Task<AssignmentWithUsersResponse> UpdateAssignmentAsync(int assignmentId, AssignmentUpdateRequest request, CancellationToken ct)
    {
        Guard.Against.Null(request, nameof(request));

        var assignment = await GetTrackedAssignmentAsync(assignmentId);

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            assignment.Name = request.Name;
        }

        if (request.Description is not null)
        {
            assignment.Description = request.Description;
        }

        if (request.PlannedTime.HasValue)
        {
            assignment.PlannedTime = request.PlannedTime.Value;
        }

        if (request.Type is not null)
        {
            assignment.Type = request.Type;
        }

        if (request.Value is not null)
        {
            assignment.Value = request.Value;
        }

        await _repository.SaveChangesAsync();

        return await MapWithUsersAsync(assignment, ct);
    }

    public async Task<AssignmentWithUsersResponse> UpdateAssignmentStatusAsync(int assignmentId, AssignmentStatus status, CancellationToken ct)
    {
        var assignment = await GetTrackedAssignmentAsync(assignmentId);
        assignment.Status = status;

        if (status == AssignmentStatus.Completed)
        {
            assignment.CompletedAt ??= _timeProvider.GetUtcNow().UtcDateTime;
        }
        else
        {
            assignment.CompletedAt = null;
        }

        await _repository.SaveChangesAsync();

        return await MapWithUsersAsync(assignment, ct);
    }

    public async Task<AssignmentWithUsersResponse> AddAssigneesAsync(int assignmentId, IEnumerable<string> userIds, CancellationToken ct)
    {
        var assignment = await GetTrackedAssignmentAsync(assignmentId);

        await AddAssigneesToAssignmentAsync(assignment, userIds, ct);

        await _repository.SaveChangesAsync();

        return await MapWithUsersAsync(assignment, ct);
    }

    public async Task<AssignmentWithUsersResponse> RemoveAssigneeAsync(int assignmentId, string userId, CancellationToken ct)
    {
        Guard.Against.NullOrWhiteSpace(userId, nameof(userId));

        var assignment = await GetTrackedAssignmentAsync(assignmentId);

        var existing = assignment.AssignmentUsers
            .FirstOrDefault(x => string.Equals(x.UserId, userId, StringComparison.OrdinalIgnoreCase));

        Guard.Against.Null(existing, nameof(userId), $"User '{userId}' is not assigned to assignment {assignmentId}.");

        assignment.AssignmentUsers.Remove(existing);

        await _repository.SaveChangesAsync();

        return await MapWithUsersAsync(assignment, ct);
    }

    public async Task DeleteAssignmentAsync(int assignmentId, CancellationToken ct)
    {
        var assignment = await GetTrackedAssignmentAsync(assignmentId);
        _repository.Remove(assignment);
        await _repository.SaveChangesAsync();
    }

    private async Task<Assignment> GetTrackedAssignmentAsync(int assignmentId)
    {
        var assignment = await _repository.GetByIdTrackedAsync(assignmentId);
        Guard.Against.NotFound(assignmentId, assignment);
        return assignment!;
    }

    private async Task<AssignmentWithUsersResponse> MapWithUsersAsync(Assignment assignment, CancellationToken ct)
    {
        var response = _mapper.Map<AssignmentWithUsersResponse>(assignment);
        var userIds = assignment.AssignmentUsers
            .Select(x => x.UserId)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (userIds.Count == 0)
        {
            response.Assignees = new List<UserResponse>();
            return response;
        }

        var users = await _userManager.Users
            .Where(x => userIds.Contains(x.Id))
            .ToListAsync(ct);

        response.Assignees = _mapper.Map<List<UserResponse>>(users);
        return response;
    }

    private async Task AddAssigneesToAssignmentAsync(Assignment assignment, IEnumerable<string> userIds, CancellationToken ct)
    {
        var distinctUserIds = userIds
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => id.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (distinctUserIds.Count == 0)
        {
            return;
        }

        var existing = assignment.AssignmentUsers
            .Select(x => x.UserId)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var userId in distinctUserIds)
        {
            if (existing.Contains(userId))
            {
                continue;
            }

            var user = await _userManager.FindByIdAsync(userId);
            Guard.Against.Null(user, nameof(userId), $"User '{userId}' was not found.");

            assignment.AssignmentUsers.Add(new AssignmentUser
            {
                AssignmentId = assignment.Id,
                Assignment = assignment,
                UserId = user.Id,
                AssignedAt = _timeProvider.GetUtcNow().UtcDateTime
            });

            existing.Add(user.Id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.BLL.Common.Interfaces.Repository;
using DispatcherApp.Models.Context;
using DispatcherApp.Models.DTOs.Assignment;
using DispatcherApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DispatcherApp.BLL.Common.Services;
public class AssignmentService : IAssignmentService
{
    public readonly IUserContextService _userContextService;
    public readonly UserManager<IdentityUser> _userManager;
    public readonly IAssignmentRepository _context;
    public readonly IMapper _mapper;

    public AssignmentService(
        IUserContextService userContextService,
        UserManager<IdentityUser> userManager,
        IMapper mapper,
        IAssignmentRepository context
        )
    {
        _userContextService = userContextService;
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }

    public async Task<IEnumerable<AssignmentResponse>> GetAssignmentListAsync()
    {
        var assignments = await _context.GetAllAsync();
        return _mapper.Map<IEnumerable<AssignmentResponse>>(assignments);
    }
    private async Task<IEnumerable<IdentityUser>> GetAssignmentUsers(int assignmentId)
    {
        var assigmnment = await _context
            .GetByIdAsync(assignmentId);
        Guard.Against.NotFound(assignmentId, assigmnment);

        var assignmentUsers = assigmnment.AssignmentUsers.Select(x => x.UserId);
        var users = await _userManager.Users.Where(x => assignmentUsers.Contains(x.Id)).ToListAsync();
        return users;
    }
    public async Task<AssignmentWithUsersResponse> GetAssignmentAsync(int id)
    {
        var assignment = await _context.GetByIdAsync(id);
        Guard.Against.NotFound(id, assignment);

        var result = _mapper.Map<AssignmentWithUsersResponse>(assignment);
        var users = await GetAssignmentUsers(assignment.Id);
        result.Assignees = _mapper.Map<List<UserResponse>>(users);
        return result;
    }
    public async Task<IEnumerable<AssignmentResponse>> GetUserAssignmentAsync()
    {
        var userId = Guard.Against.NullOrEmpty(GetCurrentUserId()?.UserId, nameof(GetCurrentUserId));

        var assignment = await _context.GetUserAssignments(userId);

        return _mapper.Map<IEnumerable<AssignmentResponse>>(assignment);
    }
    public UserContext GetCurrentUserId() => _userContextService.GetCurrentUser() ?? 
        throw new UnauthorizedAccessException("no User Context");


}

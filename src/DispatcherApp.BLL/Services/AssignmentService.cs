using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using DispatcherApp.BLL.Interfaces;
using DispatcherApp.DAL.Data;
using DispatcherApp.Models.Context;
using DispatcherApp.Models.DTOs.Assignment;
using DispatcherApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DispatcherApp.BLL.Services;
public class AssignmentService : IAssignmentService
{
    public readonly IUserContextService _userContextService;
    public readonly UserManager<IdentityUser> _userManager;
    public readonly AppDbContext _context;
    public readonly IMapper _mapper;

    public AssignmentService(
        IUserContextService userContextService,
        UserManager<IdentityUser> userManager,
        IMapper mapper,
        AppDbContext context
        )
    {
        _userContextService = userContextService;
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }

    public async Task<IEnumerable<AssignmentResponse>> GetAssignmentListAsync()
    {
        var assignments = await _context.Assignments

            .AsNoTracking()
            .Include(IQueryable => IQueryable.AssignmentUsers)
            .ToListAsync();
        return _mapper.Map<IEnumerable<AssignmentResponse>>(assignments);
    }
    private IQueryable<IdentityUser> GetAssignmentUsers(int assignmentId)
    {
        var assignmentUsers = _context.AssignmentUsers
            .AsNoTracking()
            .Where(a => a.AssignmentId == assignmentId)
            .Select(a => a.UserId);

        var users = _userManager.Users.Where(x => assignmentUsers.Contains(x.Id));
        return users;
    }
    public async Task<AssignmentWithUsersResponse> GetAssignmentAsync(int id)
    {
        var assignment = await _context.Assignments
            .AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        Guard.Against.NotFound(id, assignment);

        var result = _mapper.Map<AssignmentWithUsersResponse>(assignment);
        var users = await GetAssignmentUsers(assignment.Id).ToListAsync();
        result.Assignees = _mapper.Map<List<UserResponse>>(users);
        return result;
    }
    public async Task<IEnumerable<AssignmentResponse>> GetUserAssignmentAsync()
    {
        var userId = Guard.Against.NullOrEmpty(GetCurrentUserId()?.UserId, nameof(GetCurrentUserId));

        var assignment = await _context.Assignments
            .AsNoTracking()
            .Include(a => a.AssignmentUsers)
            .Where(a => a.AssignmentUsers.Select(x => x.UserId).Contains(userId))
            .ToListAsync();

        return _mapper.Map<IEnumerable<AssignmentResponse>>(assignment);
    }
    public UserContext GetCurrentUserId() => _userContextService.GetCurrentUser() ?? 
        throw new UnauthorizedAccessException("no User Context");


}

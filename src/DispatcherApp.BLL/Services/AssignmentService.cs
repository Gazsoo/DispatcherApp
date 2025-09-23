using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispatcherApp.BLL.Interfaces;
using DispatcherApp.Models.Context;

namespace DispatcherApp.BLL.Services;
public class AssignmentService : IAssignmentService
{
    public readonly IUserContextService _userContextService;
    public AssignmentService(IUserContextService userContextService)
    {
        _userContextService = userContextService;
    }
    public UserContext? GetCurrentUserId() => _userContextService.GetCurrentUser();
}

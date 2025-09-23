using DispatcherApp.Models.Context;

namespace DispatcherApp.BLL.Interfaces;
public interface IAssignmentService
{
    UserContext? GetCurrentUserId();
}
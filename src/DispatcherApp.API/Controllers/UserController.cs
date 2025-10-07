using DispatcherApp.BLL.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DispatcherApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;
    public UserController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [Authorize]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var a = _assignmentService.GetCurrentUserId();
        return Ok(new { username = "sampleuser", email = a?.Roles });
        }
}

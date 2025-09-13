using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DispatcherApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    // GET: HomeController
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(new { cucc = "ahham" });
    }

}

using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DirectorController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
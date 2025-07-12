using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

public class AboutController : Controller
{
    [Route("/about")]
    public IActionResult About()
    {
        return View();
    }
}
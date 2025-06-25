using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

public class UserController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Update()
    {
        return View();
    }
}
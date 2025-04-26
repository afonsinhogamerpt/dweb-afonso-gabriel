using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

public class AuthController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }
    
    public IActionResult Registo()
    {
        return View();
    }
}
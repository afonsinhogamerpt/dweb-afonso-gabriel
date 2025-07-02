using dweb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

public class UserController : Controller
{
    
    private readonly UserManager<Utilizador> _userManager;

    public UserController(UserManager<Utilizador> userManager)
    {
        _userManager = userManager;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> Update()
    {
        var user = await _userManager.GetUserAsync(User);

        
        if (user is not Utilizador utilizador)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(utilizador); 
    }
}
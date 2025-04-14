using dweb.Data;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

public class UtilizadorController : Controller
{
    private readonly AppDbContext _context;
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
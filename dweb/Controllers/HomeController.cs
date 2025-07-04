using System.Diagnostics;
using dweb.Data;
using Microsoft.AspNetCore.Mvc;
using dweb.Models;
using Microsoft.IdentityModel.Tokens;

namespace dweb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Pesquisar(string query)
    {
        ViewData["SearchTerm"] = query.IsNullOrEmpty() ? "" : query;
        return View();
    }
    


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
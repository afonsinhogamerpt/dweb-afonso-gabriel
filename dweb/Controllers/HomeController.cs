using System.Diagnostics;
using System.Security.Claims;
using dweb.Data;
using Microsoft.AspNetCore.Mvc;
using dweb.Models;
using Microsoft.IdentityModel.Tokens;

using dweb.Data;
using Microsoft.EntityFrameworkCore;


namespace dweb.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context) : base(context)
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
    
    public IActionResult Pesquisar(int? generoId, string search,string query, int page = 1)
    {
        string filmepesquisa = !string.IsNullOrEmpty(query) ? query : search;
        ViewData["SearchTerm"] = filmepesquisa ?? "";
        int pageSize = 6;
        var generos = _context.Genero.ToList();
        var filmesQuery = _context.Filme.Include(f => f.Genero).AsQueryable();
        if (generoId.HasValue)
        {
            filmesQuery = filmesQuery.Where(f => f.Genero.Any(g => g.generoID == generoId));
        }
        if (!string.IsNullOrEmpty(filmepesquisa))
        {
            filmesQuery = filmesQuery.Where(f => f.nome.ToLower().Contains(filmepesquisa.ToLower()));
        }
        var filmes = filmesQuery.ToList();
        ViewBag.Generos = generos;
        ViewBag.SelectedGenero = generoId;
        ViewBag.Search = filmepesquisa;
        ViewBag.Page = page;
        ViewBag.PageSize = pageSize;
        return View(filmes);
    }
    


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
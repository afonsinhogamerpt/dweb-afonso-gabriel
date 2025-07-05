using dweb.Models;
using dweb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dweb.Controllers;

public class UserController : BaseController
{
    
    private readonly UserManager<Utilizador> _userManager;
    private readonly AppDbContext _context;

    public UserController(UserManager<Utilizador> userManager, AppDbContext context) : base(context)
    {
        _userManager = userManager;
        _context = context;
    }
   
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

    [HttpPost]
    public async Task<IActionResult> GuardarFilme(int filmeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        var filme = await _context.Filme.FindAsync(filmeId);
        if (filme == null)
        {
            return NotFound();
        }
        if (user.Filmes == null)
            user.Filmes = new List<Filme>();
        if (!user.Filmes.Any(f => f.filmeID == filmeId))
        {
            user.Filmes.Add(filme);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("FilmeDetails", "Filme", new { id = filmeId });
    }
    
    [HttpGet]
    public async Task<IActionResult> UserFilms()
    {
        var user = await _userManager.GetUserAsync(User);
       if (user == null)
        {
            return Unauthorized();
        }
        var utilizadorComFilmes = _context.Utilizador
            .Include(u => u.Filmes)
            .FirstOrDefault(u => u.Id == user.Id);

        var filmes = utilizadorComFilmes?.Filmes?.ToList() ?? new List<Filme>();
        return View(filmes);
    }
    [HttpPost]
    public async Task<IActionResult> RetirarFilme(int filmeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        
        var utilizadorComFilmes = _context.Utilizador
            .Include(u => u.Filmes)
            .FirstOrDefault(u => u.Id == user.Id);
        if (utilizadorComFilmes == null)
        {
            return Unauthorized();
        }
        var filme = await _context.Filme.FindAsync(filmeId);
        if (filme == null)
        {
            return NotFound();
        }
        if (utilizadorComFilmes.Filmes != null && utilizadorComFilmes.Filmes.Any(f => f.filmeID == filmeId))
        {
            utilizadorComFilmes.Filmes.Remove(filme);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("FilmeDetails", "Filme", new { id = filmeId });
    }
}
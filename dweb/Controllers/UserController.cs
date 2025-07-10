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
   
    public IActionResult Users()
    {
        return View();
    }
    
    public async Task<IActionResult> Update(string id)
    {
        Utilizador utilizador;

        if (!string.IsNullOrEmpty(id))
        {
            utilizador = await _userManager.FindByIdAsync(id);
            if (utilizador == null)
              {
                  return RedirectToAction("Login", "Account");
              }
            return View(utilizador);
        }
        utilizador = await _userManager.GetUserAsync(User);

        if (utilizador == null)
        {
            return RedirectToAction("Login", "Account");
        }
        
        return View(utilizador);
        
    }

    [HttpPost]
    public async Task<IActionResult> GuardarFilme(int filmeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var filme = await _context.Filme.FindAsync(filmeId);
        if (filme == null) return NotFound();

        var existing = await _context.FilmeUtilizador
            .FirstOrDefaultAsync(fu => fu.FilmeId == filmeId && fu.UtilizadorId == user.Id);

        if (existing == null)
        {
            var filmeUtilizador = new FilmeUtilizador
            {
                FilmeId = filmeId,
                UtilizadorId = user.Id,
                IsGuardado = true,
                IsLike = false
            };

            _context.FilmeUtilizador.Add(filmeUtilizador);
        }
        else if (!existing.IsGuardado)
        {
            existing.IsGuardado = true;
            _context.FilmeUtilizador.Update(existing);
        }

        await _context.SaveChangesAsync();

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

        var filmes = await _context.FilmeUtilizador
            .Where(fu => fu.UtilizadorId == user.Id)
            .Include(fu => fu.Filme)
            .Select(fu => fu.Filme)
            .ToListAsync();

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

        var relacao = await _context.FilmeUtilizador
            .FirstOrDefaultAsync(f => f.FilmeId == filmeId && f.UtilizadorId == user.Id);

        if (relacao != null)
        {
            _context.FilmeUtilizador.Remove(relacao);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("FilmeDetails", "Filme", new { id = filmeId });
    }
}
using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

public class GenController : BaseController 
{
    private readonly AppDbContext _context;
    
    public GenController(AppDbContext context) : base(context)
    {
        _context = context;
    }
  
    
    [Authorize (Roles = "Administrador")]
    public IActionResult Generos()
    {
        return View();
    }
    
    [HttpPost("put")]
    public async Task<ActionResult<Genero>> PutGeneroForm([FromForm] string nome, [FromForm] int generoID) 
    {
        var gen = _context.Genero. 
            Where(g => g.generoID == generoID). 
            FirstOrDefault();

        if (gen is not Genero)
        {
            return NotFound();
        }
        
        gen.nome = nome;
        _context.SaveChanges();
        return RedirectToAction("Generos", "Gen");
    }
    
    [HttpPost]
    public async Task<ActionResult<Genero>> DeleteGeneroForm([FromForm] int id)
    {
        var g = _context.Genero. 
            Where(g => g.generoID == id). 
            FirstOrDefault();

        if (g is not Genero)
        {
            return NotFound();
        }
        
        _context.Genero.Remove(g);
        await _context.SaveChangesAsync();
        return RedirectToAction("Generos", "Gen");
    }
}
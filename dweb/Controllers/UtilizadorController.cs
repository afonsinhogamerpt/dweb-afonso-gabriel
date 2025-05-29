using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UtilizadorController : Controller
{
    private readonly AppDbContext _context;

    public UtilizadorController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizadores()
    {
        var utilizadores = _context.Utilizador.ToList();
        return Ok(utilizadores);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Utilizador>> GetUtilizador(string id)
    {
        var utilizador =  _context.Utilizador.Where(u => u.Id.Equals(id)). 
            FirstOrDefault();

        if (utilizador is not Utilizador)
        {
            return NotFound();
        }
        
        return Ok(utilizador);
    }

    [HttpPut]
    public async Task<IActionResult> PutUtilizador(Utilizador utilizador)
    {
        var u = _context.Utilizador.
            Where(u => u.Id.Equals(utilizador.Id)).
            FirstOrDefault();

        if (u is not Utilizador)
        {
            return NotFound();
        }
        
        u.Email = utilizador.Email;
        await _context.SaveChangesAsync();
        return Ok(utilizador);
    }

    [HttpPost]
    public async Task<IActionResult> PostUtilizador(Utilizador utilizador)
    {
        
        _context.Utilizador.Add(utilizador);
        await _context.SaveChangesAsync();
        return Ok(utilizador);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUtilizador([FromBody] string id)
    {
        var u = _context.Utilizador. 
            Where(u => u.Id.Equals(id)). 
            FirstOrDefault();

        if (u == null)
        {
            return NotFound();
        }
        
        _context.Utilizador.Remove(u);
        await _context.SaveChangesAsync();
        return Ok("Utilizador removido com sucesso!");
    }
}
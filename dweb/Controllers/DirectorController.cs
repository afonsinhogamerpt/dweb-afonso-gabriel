using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DirectorController : Controller
{
    
    private readonly AppDbContext _context;

    public DirectorController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Director>>> GetDirectors()
    {
        var directors =  _context.Director.ToList();
        return directors;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDirectors(int id)
    {
        var director =  _context.Director.
            Where(d => d.directorID == id).
            FirstOrDefault();

        if (director is Director)
        {
            return Ok(director);
        }
        
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> PostDirector(Director director)
    {
        
        _context.Director.Add(director);
        await _context.SaveChangesAsync();
        return Ok(director);
        
    }

    [HttpPut]
    public async Task<IActionResult> PutDirector(Director director)
    {
        var dir = _context.Director. 
            Where(d => d.directorID == director.directorID). 
            FirstOrDefault();

        if (dir is not Director)
        {
            return NotFound();
        }
        dir.nome = director.nome;
        
        await _context.SaveChangesAsync();
        return Ok(director);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDirector(Director director)
    {
        var dir = _context.Director.Where(d => d.directorID == director.directorID). 
            FirstOrDefault();

        if (dir == null)
        {
            return NotFound();
        }
        
        _context.Director.Remove(dir);
        await _context.SaveChangesAsync();
        return Ok("Director removido com sucesso!");
    }
}
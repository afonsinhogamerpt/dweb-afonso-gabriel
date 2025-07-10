using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DirectorController : BaseController
{
    
    private readonly AppDbContext _context;

    public DirectorController(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    [HttpGet]
    [Route("/Director/Directors")]
    public IActionResult Directors()
    {
        var directors = _context.Director.ToList();
        return View("directors", directors);
    }

    [HttpPost("update-director")]
    public async Task<IActionResult> UpdateDirector([FromForm] int directorID, [FromForm] string nome, [FromForm] int idade, [FromForm] string bio)
    {
        var d = _context.Director.FirstOrDefault(x => x.directorID == directorID);
        if (d == null)
        {
            return NotFound();
        }
        d.nome = nome;
        d.idade = idade;
        d.bio = bio;
        await _context.SaveChangesAsync();
        return RedirectToAction("DirectorDetails", new { id = directorID });
    }

    [HttpPost("delete-director")]
    public async Task<IActionResult> DeleteDirector([FromForm] int directorID)
    {
        var director = _context.Director.FirstOrDefault(a => a.directorID == directorID);
        if (director == null)
        {
            return NotFound();
        }
        _context.Director.Remove(director);
        await _context.SaveChangesAsync();
        return RedirectToAction("Directors");
    }

    [HttpGet("all")]
    public ActionResult<IEnumerable<Director>> GetDirectors()
    {
        var directors = _context.Director.ToList();
        return directors;
    }

    [HttpGet("{id}")]
    public ActionResult<Director> GetDirector(int id)
    {
        var director = _context.Director.FirstOrDefault(a => a.directorID == id);
        if (director != null)
        {
            return Ok(director);
        }
        return NotFound();
    }

    [HttpGet]
    [Route("/Director/DirectorDetails/{id}")]
    public IActionResult DirectorDetails(int id)
    {
        var director = _context.Director.FirstOrDefault(a => a.directorID == id);
        if (director == null)
        {
            return NotFound();
        }
        return View(director);
    }

    [HttpPost]
    public async Task<ActionResult<Director>> PostDirector(Director director)
    {
        _context.Director.Add(director);
        await _context.SaveChangesAsync();
        return Ok(director);
    }

    [HttpPost("create-Director")]
    public async Task<IActionResult> CreateDirector([FromForm] string nome, [FromForm] int idade, [FromForm] string bio)
    {
        var director = new Director
        {
            nome = nome,
            idade = idade,
            bio = bio
        };
        _context.Director.Add(director);
        await _context.SaveChangesAsync();
        return RedirectToAction("Directors");
    }
}
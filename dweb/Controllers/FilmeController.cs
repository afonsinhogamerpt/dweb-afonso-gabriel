using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmeController : Controller
{
    private readonly AppDbContext _context;

    public FilmeController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Filme>>> GetFilmes()
    {
        var filmes = _context.Filme.ToList();
        return Ok(filmes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Filme>> GetFilme(int id)
    {
        var filme = _context.Filme. 
            Where(f => f.filmeID == id). 
            FirstOrDefault();

        if (filme is not Filme)
        {
            return NotFound();
        }
        
        return Ok(filme);
    }

    [HttpPost]
    public async Task<ActionResult<Filme>> PostFilme(Filme filme)
    {
        _context.Filme.Add(filme);
        await _context.SaveChangesAsync();
        return Ok(filme);
    }

    [HttpPut]
    public async Task<ActionResult<Filme>> PutFilme(Filme filme)
    {
        var f = _context.Filme.
            Where(f => f.filmeID == filme.filmeID). 
            FirstOrDefault();

        if (f is not Filme)
        {
            return NotFound();
        }
        
        f.nome = filme.nome;
        f.resumo = filme.resumo;
        await _context.SaveChangesAsync();
        return Ok(filme);
    }

    [HttpDelete]
    public async Task<ActionResult<Filme>> DeleteFilme([FromBody] int id)
    {
        var f = _context.Filme. 
            Where(f => f.filmeID == id). 
            FirstOrDefault();

        if (f == null)
        {
            return NotFound();
        }
        _context.Filme.Remove(f);
        await _context.SaveChangesAsync();
        return Ok("Filme removido com sucesso!");
    }

    // Action para exibir detalhes do filme
    [HttpGet]
    [Route("/Filme/FilmeDetails/{id}")]
    public IActionResult FilmeDetails(int id)
    {
        var filme = _context.Filme.FirstOrDefault(f => f.filmeID == id);
        if (filme == null)
        {
            return NotFound();
        }
        return View(filme);
    }
}
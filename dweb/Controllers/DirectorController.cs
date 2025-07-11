using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Authorization;
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
    
    /// <summary>
    /// Redireciona para a View "Directors"
    /// </summary>
    /// <returns>
    /// Retorna a View "Directors" com um objeto que contém uma lista de directores
    /// </returns>
    [Authorize (Roles = "Administrador")]
    [HttpGet]
    [Route("/Director/Directors")]
    public IActionResult Directors()
    {
        var directors = _context.Director.ToList();
        return View("directors", directors);
    }

    /// <summary>
    /// Atualiza os dados de um determinado director (este método é utilizado num form)
    /// </summary>
    /// <returns>
    /// Redireciona para a View "DirectorDetails" em função do directorID
    /// </returns>
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

    /// <summary>
    /// Apaga os dados de um determinado director (este método é utilizado num form)
    /// </summary>
    /// <returns>
    /// Redireciona para a View "Directors"
    /// </returns>
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

    /// <summary>
    /// Lista todos os directores da aplicação
    /// </summary>
    /// <returns>
    ///Retorna um objeto com uma lista de directores
    /// </returns>
    [HttpGet("all")]
    public ActionResult<IEnumerable<Director>> GetDirectors()
    {
        var directors = _context.Director.ToList();
        return directors;
    }

    /// <summary>
    /// Retorna um director dado um determinado id passado por argumento
    /// </summary>
    /// <returns>
    ///Retorna um objeto com o director que dê match ao id especificado
    /// </returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    ///Retorna a View "DirectorDetails"
    /// </returns>
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

    /// <summary>
    /// Cria um novo director na database
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<Director>> PostDirector(Director director)
    {
        _context.Director.Add(director);
        await _context.SaveChangesAsync();
        return Ok(director);
    }

    /// <summary>
    /// Cria um novo director na database (este método é utilizado num form)
    /// </summary>
    /// <returns>
    ///Redireciona para a View "Directors"
    /// </returns>
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
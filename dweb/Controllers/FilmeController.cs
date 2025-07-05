using dweb.Data;
using dweb.Models;
using dweb.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace dweb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmeController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<Utilizador> _userManager;

    public FilmeController(AppDbContext context, UserManager<Utilizador> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FilmeDTO>>> GetFilmes()
    {
        var filmes = await _context.Filme
            .Include(f => f.Genero)
            .Include(f => f.Director)
            .Include(f => f.Actor)
            .Select(f => new FilmeDTO
            {
                filmeID = f.filmeID,
                nome = f.nome,
                resumo = f.resumo,
                imagem = f.imagem,
                ano = f.ano,
                generos = f.Genero.Select(g => new GeneroDTO
                {
                    generoID = g.generoID,
                    nome = g.nome
                }).ToList(),
                
                directores = f.Director.Select(g => new DirectorDTO
                {
                    directorID = g.directorID,
                    nome = g.nome,
                    idade = g.idade, 
                    bio = g.bio,  
                    imagem = g.imagem,
                        
                }).ToList(),
                actores = f.Actor.Select(a => new ActorDTO
                {
                    actorID = a.actorID,
                    nome = a.nome,
                    idade = a.idade,
                    bio = a.bio,
                    imagem = a.imagem
                }).ToList()
            })
            .ToListAsync();

        return Ok(filmes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<FilmeDTO>>> GetFilme(int id)
    {
        var filme = _context.Filme 
            .Include(f => f.Genero)
            .Include(f => f.Director)
            .Include(f => f.Actor).
            Select(f => new FilmeDTO
            {
                filmeID = f.filmeID,
                nome = f.nome,
                resumo = f.resumo,
                imagem = f.imagem,
                ano = f.ano,
                generos = f.Genero.Select(g => new GeneroDTO
                {
                    generoID = g.generoID,
                    nome = g.nome
                }).ToList(),
                
                directores = f.Director.Select(g => new DirectorDTO
                {
                    directorID = g.directorID,
                    nome = g.nome,
                    idade = g.idade, 
                    bio = g.bio,  
                    imagem = g.imagem,
                        
                }).ToList(),
                actores = f.Actor.Select(a => new ActorDTO
                {
                    actorID = a.actorID,
                    nome = a.nome,
                    idade = a.idade,
                    bio = a.bio,
                    imagem = a.imagem
                }).ToList()
            }).
            Where(f => f.filmeID == id). 
            FirstOrDefault();

        if (filme == null)
        {
            return NotFound();
        }
        
        return Ok(filme);
    }

    [HttpPost]
    public async Task<ActionResult<Filme>> PostFilme(CreateMovieDTO createMovieDTO)
    {

        var generos = await _context.Genero.Where(g => createMovieDTO.generos.Contains(g.generoID)).ToListAsync();
        var directores = await _context.Director.Where(d => createMovieDTO.directores.Contains(d.directorID)).ToListAsync();
        var actores = await _context.Actor.Where(a => createMovieDTO.actores.Contains(a.actorID)).ToListAsync();
        
        var filme = new Filme
        {
            nome = createMovieDTO.nome,
            resumo = createMovieDTO.resumo,
            imagem = createMovieDTO.imagem,
            ano = createMovieDTO.ano,
            Genero = generos,
            Director = directores,
            Actor = actores,
        };
        
        var response = new MovieDTO
        {
            nome = filme.nome,
            resumo = filme.resumo,
            ano = filme.ano,
            imagem = filme.imagem,
            Generos = generos.Select(g => g.generoID).ToList(),
            Directores = directores.Select(d => d.directorID).ToList(),
            Actores = actores.Select(a => a.actorID).ToList(),
        };
        
        _context.Filme.Add(filme);
        await _context.SaveChangesAsync();
        return Ok(response);
        
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

    [HttpGet]
    [Route("/Filme/FilmeDetails/{id}")]
    public async Task<IActionResult> FilmeDetails(int id)
    {
        var filme = _context.Filme
            .Include(f => f.Actor)
            .Include(f => f.Director)
            .FirstOrDefault(f => f.filmeID == id);
        if (filme == null)
        {
            return NotFound();
        }

        ViewBag.Directores = filme.Director.ToList();
        ViewBag.Actores = filme.Actor.ToList();
        bool filmeGuardado = false;
        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var utilizador = await _context.Utilizador
                    .Include(u => u.Filmes)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);
                if (utilizador?.Filmes != null)
                {
                    filmeGuardado = utilizador.Filmes.Any(f => f.filmeID == id);
                }
            }
        }
        ViewBag.FilmeGuardado = filmeGuardado;
        return View(filme);
    }
}
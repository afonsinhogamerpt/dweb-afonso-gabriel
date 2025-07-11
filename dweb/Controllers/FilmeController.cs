using System.Security.Claims;
using dweb.Data;
using dweb.Models;
using dweb.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace dweb.Controllers;

    [Route("api/[controller]")]
    [ApiController]
public class FilmeController : BaseController
{
    private readonly AppDbContext _context;
    private readonly UserManager<Utilizador> _userManager;
    
    public FilmeController(AppDbContext context, UserManager<Utilizador> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }
    /// <summary>
    /// Redireciona para a View "Films" e é feita a passagem de dados do controller para a view com ViewBags
    /// </summary>
    /// <returns>
    ///Redireciona para a View "Films"
    /// </returns>
    [Authorize (Roles = "Administrador")]
    [HttpGet]
    [Route("/Filme/Films")]
    public IActionResult Films()
    {
        var filmes = _context.Filme.Include(f => f.Actor).Include(f => f.Director).ToList();
        var actors = _context.Actor.ToList();
        var directors = _context.Director.ToList();
        var generos = _context.Genero.ToList();
        ViewBag.Actors = actors;
        ViewBag.Directors = directors;
        ViewBag.Generos = generos;
        return View("Films", filmes);
    }
    
    
    /// <summary>
    /// Redireciona para a View "Films" e é feita a passagem de dados do controller para a view com ViewBags
    /// </summary>
    /// <returns>
    ///Redireciona para a View "FilmeDetails"
    /// </returns>

    [HttpPost("update-filme")]
    public async Task<IActionResult> UpdateFilme([FromForm] int filmeID, [FromForm] string nome, [FromForm] string resumo, [FromForm] int ano,[FromForm] List<int> actores,
        [FromForm] List<int> directores,
        [FromForm] List<int> generos, IFormFile? file)
    {
        var f = _context.Filme.Include(x => x.Actor).Include(x => x.Director).Include(x => x.Genero).FirstOrDefault(x => x.filmeID == filmeID);
        
        if (f == null)
        {
            return NotFound();
        }
        f.nome = nome;
        f.resumo = resumo;
        f.ano = ano;

        f.Actor.Clear();
        if (actores != null && actores.Count > 0)
        {
            var selectedActores = await _context.Actor.Where(a => actores.Contains(a.actorID)).ToListAsync();
            foreach (var actor in selectedActores)
            {
                _context.Actor.Attach(actor);
                f.Actor.Add(actor);
            }
        }
        f.Director.Clear();
        if (directores != null && directores.Count > 0)
        {
            var selectedDirectores = await _context.Director.Where(d => directores.Contains(d.directorID)).ToListAsync();
            foreach (var director in selectedDirectores)
            {
                _context.Director.Attach(director);
                f.Director.Add(director);
            }
        }
        f.Genero.Clear();
        if (generos != null && generos.Count > 0)
        {
            var selectedGeneros = await _context.Genero.Where(g => generos.Contains(g.generoID)).ToListAsync();
            foreach (var genero in selectedGeneros)
            {
                f.Genero.Add(genero);
            }
        }
        if (file != null)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            f.imagem = memoryStream.ToArray();
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("FilmeDetails", new { id = filmeID });
    }

    /// <summary>
    /// Cria um novo filme com os dados que são passados por argumento (este método é utilizado num form) 
    /// </summary>
    /// <returns>
    ///Redireciona para a View "Films"
    /// </returns>
    [HttpPost("create-filme")]
    public async Task<IActionResult> CreateFilme([FromForm] string nome,[FromForm] string resumo, [FromForm] int ano,
        [FromForm] List<int> actores,
        [FromForm] List<int> directores,
        [FromForm] List<int> generos,
        IFormFile? file )
    {
        var selectedActores = await _context.Actor.Where(a => actores.Contains(a.actorID)).ToListAsync();
        var selectedDirectores = await _context.Director.Where(d => directores.Contains(d.directorID)).ToListAsync();
        var selectedGeneros = await _context.Genero.Where(g => generos.Contains(g.generoID)).ToListAsync();
        var filme = new Filme
        {
            nome = nome,
            resumo = resumo,
            ano = ano,
            Actor = selectedActores,
            Director = selectedDirectores,
            Genero = selectedGeneros
        };
        if (file != null)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            filme.imagem = memoryStream.ToArray();
        }
        _context.Filme.Add(filme);
        await _context.SaveChangesAsync();
        return RedirectToAction("Films");
    }

    
    /// <summary>
    /// Lista todos os filmes da aplicação
    /// </summary>
    /// <returns>
    ///Retorna um objeto com uma lista de filmes
    /// </returns>
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

    
    /// <summary>
    /// Retorna um filme dado um determinado id passado por argumento
    /// </summary>
    /// <returns>
    ///Retorna um objeto com o filme que dê match ao id especificado
    /// </returns>
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
    
    
    /// <summary>
    /// Cria um novo filme na database
    /// </summary>
    /// <returns>
    ///
    /// </returns>
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
            Generos = generos.Select(g => g.generoID).ToList(),
            Directores = directores.Select(d => d.directorID).ToList(),
            Actores = actores.Select(a => a.actorID).ToList(),
        };

        _context.Filme.Add(filme);
        await _context.SaveChangesAsync();
        return Ok(response);

    }
    /// <summary>
    /// Cria um novo filme na database (este método é utilizado num form)
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    [HttpPost("filme-file")]
    public async Task<ActionResult<Filme>> PostFilmefile(CreateMovieDTO createMovieDTO,  IFormFile? file)
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
        
        if (file != null)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            filme.imagem = memoryStream.ToArray();
        }

        var response = new MovieDTO
        {
            nome = filme.nome,
            resumo = filme.resumo,
            ano = filme.ano,
            Generos = generos.Select(g => g.generoID).ToList(),
            Directores = directores.Select(d => d.directorID).ToList(),
            Actores = actores.Select(a => a.actorID).ToList(),
        };

        _context.Filme.Add(filme);
        await _context.SaveChangesAsync();
        return Ok(response);

    }

    /// <summary>
    /// Atualiza os dados de um filme
    /// </summary>
    /// <returns>
    ///Guarda na database os dados atualizados do filme
    /// </returns>
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
    
    /// <summary>
    /// Apaga os dados de um filme da database
    /// </summary>
    /// <returns>
    ///
    /// </returns>

    [HttpPost("delete_film")]
    public async Task<ActionResult<Filme>> DeleteFilme([FromForm] int filmeID)
    {
        var f = _context.Filme.
            Where(f => f.filmeID == filmeID).
            FirstOrDefault();

        if (f == null)
        {
            return NotFound();
        }
        _context.Filme.Remove(f);
        await _context.SaveChangesAsync();
        return RedirectToAction("Films");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    /// Retorna a View "FilmeDetails" com o filme escolhido pelo utilizador
    /// </returns>
    
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

        ViewData["likes"] = filme.likes;
        ViewData["dislikes"] = filme.dislikes;
        ViewData["filmeID"] = filme.filmeID;
        
        var actors = _context.Actor.ToList();
        var directors = _context.Director.ToList();
        var generos = _context.Genero.ToList();
        ViewBag.Actors = actors;
        ViewBag.Directors = directors;
        ViewBag.Generos = generos;
        bool filmeGuardado = false;
        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                filmeGuardado = await _context.FilmeUtilizador
                    .AnyAsync(fu => fu.FilmeId == id && fu.UtilizadorId == user.Id && fu.IsGuardado);
            }
        }
        ViewBag.FilmeGuardado = filmeGuardado;
        return View(filme);
    }
}
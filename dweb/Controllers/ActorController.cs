using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : BaseController
    {
        private readonly AppDbContext _context;

        public ActorController(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Lista todos os actores da aplicação
        /// </summary>
        /// <returns>
        ///Retorna um objeto com uma lista de actores
        /// </returns>
        [Authorize (Roles = "Administrador")]
        [HttpGet]
        [Route("/Actor/Actors")]
        public IActionResult Actors()
        {
            var actors = _context.Actor.ToList();
            return View("actors", actors);
        }
        
        /// <summary>
        /// Atualiza os dados de um actor (este método é utilizado num form)
        /// </summary>
        /// <returns>
        ///Guarda na database os dados atualizados do actor
        /// </returns>

        [HttpPost("update-actor")]
        public async Task<IActionResult> UpdateActor([FromForm] int actorID, [FromForm] string nome, [FromForm] int idade, [FromForm] string bio)
        {
            var a = _context.Actor.FirstOrDefault(x => x.actorID == actorID);
            if (a == null)
            {
                return NotFound();
            }
            a.nome = nome;
            a.idade = idade;
            a.bio = bio;
            await _context.SaveChangesAsync();
            return RedirectToAction("ActorDetails", new { id = actorID });
        }

        /// <summary>
        /// Apaga os dados de um actor (este método é utilizado num form)
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        [HttpPost("delete-actor")]
        public async Task<IActionResult> DeleteActor([FromForm] int actorID)
        {
            var actor = _context.Actor.FirstOrDefault(a => a.actorID == actorID);
            if (actor == null)
            {
                return NotFound();
            }
            _context.Actor.Remove(actor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Actors");
        }

        
        /// <summary>
        /// Lista todos os actores da aplicação
        /// </summary>
        /// <returns>
        ///Retorna um objeto com uma lista de actores
        /// </returns>
        [HttpGet("all")]
        public ActionResult<IEnumerable<Actor>> GetActors()
        {
            var actors = _context.Actor.ToList();
            return actors;
        }

        /// <summary>
        /// Retorna um actor dado um determinado id passado por argumento
        /// </summary>
        /// <returns>
        /// Retorna um objeto com o actor que dê match ao id especificado
        /// </returns>
        [HttpGet("{id}")]
        public ActionResult<Actor> GetActor(int id)
        {
            var actor = _context.Actor.FirstOrDefault(a => a.actorID == id);
            if (actor != null)
            {
                return Ok(actor);
            }
            return NotFound();
        }

        /// <summary>
        /// Retorna um actor dado um determinado id passado por argumento
        /// </summary>
        /// <returns>
        /// Retorna a View "ActorDetails" com o utilizador que dê match dado o id passado por argumento
        /// </returns>
        [HttpGet]
        [Route("/Actor/ActorDetails/{id}")]
        public IActionResult ActorDetails(int id)
        {
            var actor = _context.Actor.FirstOrDefault(a => a.actorID == id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        /// <summary>
        /// Cria um novo actor na database
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Actor>> PostActor(Actor actor)
        {
            _context.Actor.Add(actor);
            await _context.SaveChangesAsync();
            return Ok(actor);
        }

        /// <summary>
        /// Cria um novo actor na database (este método é utilizado num form)
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        [HttpPost("create-Actor")]
        public async Task<IActionResult> CreateActor([FromForm] string nome, [FromForm] int idade, [FromForm] string bio)
        {
            var actor = new Actor
            {
                nome = nome,
                idade = idade,
                bio = bio
            };
            _context.Actor.Add(actor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Actors");
        }
    }
}
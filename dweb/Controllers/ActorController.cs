using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ActorController : Controller
{
    
    private readonly AppDbContext _context;

    public ActorController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Actor>>> GetActors()
    {
        var actors =  _context.Actor.ToList();
        return actors;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Actor>> GetActor(int id)
    {
        var actor = _context.Actor.Where(a => a.actorID == id).
            FirstOrDefault();

        if (actor is Actor)
        {
            return Ok(actor);
        }
        return NotFound();
    }
    
    [HttpPost]
    public async Task<ActionResult<Actor>> PostActor(Actor actor)
    {
        
        _context.Actor.Add(actor);
        await _context.SaveChangesAsync();
        return Ok(actor);
    }

    
    [HttpPut("{id}")]
    public async Task<ActionResult<Actor>> PutActor(Actor actor)
    {
        var a =  _context.Actor.
                Where(a => a.actorID == actor.actorID).
                FirstOrDefault();
        
        if (a == null)
        {
            return NotFound();
        }
        
        a.nome = actor.nome;
        a.idade = actor.idade;
        a.bio = actor.bio;
        
        //_context.Actor.Update(actor);
        await _context.SaveChangesAsync();
        return Ok(actor);
    }

    [HttpDelete]
    public async Task<ActionResult<Actor>> DeleteActor([FromBody] int id)
    {
        var act = _context.Actor.Where(a => a.actorID == id). 
                FirstOrDefault();

        if (act == null)
        {
            return NotFound();
        }
        
        _context.Actor.Remove(act);
        await _context.SaveChangesAsync();
        return Ok("Actor removido com sucesso!");
    }
}
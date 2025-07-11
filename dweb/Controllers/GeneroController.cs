﻿using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeneroController : BaseController
{
    
    private readonly AppDbContext _context;

    public GeneroController(AppDbContext context) : base(context)
    {
        _context = context;
    }
    /// <summary>
    /// Mostra todos os géneros
    /// </summary>
    /// <returns>
    ///Retorna um objeto com a lista de géneros disponíveis
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genero>>> GetGenero()
    {
        var generos = _context.Genero.
            ToList();
        return Ok(generos);
    }
    
    /// <summary>
    /// Mostra um género dado um id
    /// </summary>
    /// <returns>
    ///Retorna um objeto com o género
    /// </returns>

    [HttpGet("{id}")]
    public async Task<ActionResult<Genero>> GetGenero(int id)
    {
        var genero = _context.Genero. 
                Where(g => g.generoID == id).
                FirstOrDefault();

        if (genero == null)
        {
            return NotFound();
        }
        
        
        return Ok(genero);
    }
    
    /// <summary>
    /// Adiciona um género à database
    /// </summary>
    /// <returns>
    ///
    /// </returns>

    [HttpPost]
    public async Task<ActionResult<Genero>> PostGenero(Genero genero)
    {
        _context.Genero.Add(genero);
        await _context.SaveChangesAsync();
        return Ok(genero);
    }
    
    /// <summary>
    /// Adiciona um género à database (este método é utilizado num form)
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    
    [HttpPost("form")]
    public async Task<ActionResult<Genero>> PostGeneroForm([FromForm] string nome)
    {
        var genero = new Genero
        {
            nome = nome
        };
        
        _context.Genero.Add(genero);
        await _context.SaveChangesAsync();
        return RedirectToAction("Generos", "Gen");
    }
    
    
    /// <summary>
    /// Atualiza um determinado género
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    [HttpPut]
    public async Task<ActionResult<Genero>> PutGenero(Genero genero)
    {
        var gen = _context.Genero. 
            Where(g => g.generoID == genero.generoID). 
            FirstOrDefault();

        if (gen is not Genero)
        {
            return NotFound();
        }
    
        gen.nome = genero.nome;
        _context.SaveChanges();
        return Ok(gen);
    }
    
    
    /// <summary>
    /// Apaga um determinado género
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    [HttpDelete]
    public async Task<ActionResult<Genero>> DeleteGenero([FromBody] int id)
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
        return Ok("Género removido com sucesso!");
    }
    
}
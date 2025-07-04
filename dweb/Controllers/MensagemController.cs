﻿using dweb.Data;
using dweb.Models;
using dweb.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace dweb.Controllers;

[Route("api/[controller]")]
[ApiController]

public class MensagemController : Controller
{
    private readonly AppDbContext _context;

    public MensagemController(AppDbContext context)
    {
        _context = context;
    }
    
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MensagemOutputDTO>>> GetMensagens()
    {
        var mensagens = await _context.Mensagem.
            Include(m => m.User).
            OrderBy(m => m.timestamp).
            Select(m => new MensagemOutputDTO
            {
                mensagemID = m.mensagemID,
                conteudo = m.conteudo,
                timestamp = m.timestamp,
                UserID = m.UserID,
                username = m.User.UserName
            }).
            ToListAsync();
        
        return Ok(mensagens);
    }
    
    [HttpPut]
    public async Task<IActionResult> PutMensagens(Mensagem mensagem)
    {
        
        var m = await _context.Mensagem. 
            Where(me => me.mensagemID == mensagem.mensagemID).
            FirstOrDefaultAsync();
        
        if (m is not Mensagem)
        {
            return NotFound();
        }
        m.conteudo = mensagem.conteudo;
        m.timestamp = DateTime.Now;
        
        await _context.SaveChangesAsync();
        return Ok(m);
    }

    [HttpPost]
    public async Task<IActionResult> PostMensagens([FromBody] MensagemDTO mensagem)
    {

        var user = await _context.Users.FindAsync(mensagem.UserID);
        
        if (user == null)
        {
            return Ok();
        }
        var me = new Mensagem
        {
            conteudo = mensagem.conteudo,
            timestamp = DateTime.Now,
            User = user,
            UserID = mensagem.UserID
        };
        
        _context.Mensagem.Add(me);
        await _context.SaveChangesAsync();
        return Ok(mensagem);
    }
    
    
    [HttpDelete]
    public async Task<IActionResult> DeleteMensagens(Mensagem mensagem)
    {
        var me = await _context.Mensagem. 
            Where(me => me.mensagemID == mensagem.mensagemID). 
            FirstOrDefaultAsync();

        if (me is not Mensagem)
        {
            return NotFound();
        }
        
        _context.Mensagem.Remove(me);
        await _context.SaveChangesAsync();
        return Ok("Mensagem apagada com sucesso!");
    }
}
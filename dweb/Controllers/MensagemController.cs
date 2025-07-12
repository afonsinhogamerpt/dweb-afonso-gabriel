using dweb.Data;
using dweb.Models;
using dweb.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace dweb.Controllers;

[Route("api/[controller]")]
[ApiController]

public class MensagemController : BaseController
{
    private readonly AppDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext;

    public MensagemController(AppDbContext context, IHubContext<ChatHub> hubContext) : base(context)
    {
        _context = context;
        _hubContext = hubContext;
    }
    
    /// <summary>
    /// Retorna uma mensagem dado o id passado por argumento
    /// </summary>
    /// <returns>
    ///Retorna uma mensagem
    /// </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<MensagemOutputDTO>>> GetMensagem(int id)
    {
        var mensagem = await _context.Mensagem.Where(u => u.mensagemID.Equals(id)). 
            FirstOrDefaultAsync();

        
        return Ok(mensagem);
    }
    
    /// <summary>
    /// Lista todas as mensagens da aplicação
    /// </summary>
    /// <returns>
    ///Retorna um objeto com uma lista de mensagens
    /// </returns>
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
    
    /// <summary>
    /// Atualiza os dados de uma mensagem
    /// </summary>
    /// <returns>
    ///Guarda na database os dados atualizados da mensagem
    /// </returns>
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
    
    
    /// <summary>
    /// Cria uma nova mensagem na database e propaga-a via ws (signalR) para todos os clients
    /// </summary>
    /// <returns>
    ///
    /// </returns>
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

        
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", new {
            conteudo = me.conteudo,
            timestamp = me.timestamp,
            userID = me.UserID,
            username = user.UserName
        });

        return Ok(mensagem);
    }
    
    /// <summary>
    /// Cria uma nova mensagem na database
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    [HttpPost("no-signalr")]
    [Route("no-signalr")]
    public async Task<IActionResult> PostMensagensnd([FromBody] MensagemDTO mensagem)
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
    
    /// <summary>
    /// Apaga uma  mensagem da database
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    /// 
    [HttpDelete]
    public async Task<IActionResult> DeleteMensagens([FromBody] Mensagem mensagem)
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
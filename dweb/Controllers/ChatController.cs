using System.Security.Claims;
using dweb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dweb.Controllers;

public class ChatController : BaseController
{
    
    private readonly AppDbContext _context;
    public ChatController(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    ///Retorna a View "Chat" com um objeto com uma lista de mensagens
    /// </returns>
    [Route("chat")]
    public async Task<IActionResult> Chat()
    {
        var mensagens = await _context.Mensagem. 
            Include(u => u.User ). 
            OrderBy(u => u.timestamp). 
            ToListAsync();
        
        var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        
        //return Ok(userId);
        ViewData["UserID"] = userId;
        
        return View(mensagens);
    }
}
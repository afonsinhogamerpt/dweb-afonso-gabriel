using dweb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dweb.Controllers;

public class ChatController : Controller
{
    
    private readonly AppDbContext _context;
    public ChatController(AppDbContext context)
    {
        _context = context;
    }
    
    [Route("chat")]
    public async Task<IActionResult> Chat()
    {
        var mensagens = await _context.Mensagem. 
            Include(u => u.User ). 
            OrderBy(u => u.timestamp). 
            ToListAsync();
        return View(mensagens);
    }
}
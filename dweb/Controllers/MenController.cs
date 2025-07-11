using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

public class MenController : BaseController
{
    
    private readonly AppDbContext _context;
    private readonly UserManager<Utilizador> _userManager;

    public MenController(AppDbContext context, UserManager<Utilizador> userManager) : base(context)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteMensagemForm([FromForm]int id)
    {
        var msg = await _context.Mensagem.FindAsync(id);
        if (msg == null) return NotFound();

        _context.Mensagem.Remove(msg);
        await _context.SaveChangesAsync();

        return RedirectToAction("Chat", "Chat");
    }
}
using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace dweb.Controllers;

public class ChatHub : Hub
{
    
    private readonly AppDbContext _context;
    private readonly UserManager<Utilizador> _userManager;
    
    
    public ChatHub(AppDbContext context, UserManager<Utilizador> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task SendMessage(string message)
    {
        var user = await _userManager.GetUserAsync(Context.User);

        if (user == null || string.IsNullOrWhiteSpace(message))
            return;

        var novaMensagem = new Mensagem
        {
            UserID = user.Id,
            conteudo = message,
            timestamp = DateTime.UtcNow
        };

        _context.Mensagem.Add(novaMensagem);
        await _context.SaveChangesAsync();
        
        await Clients.All.SendAsync("ReceiveMessage", user.UserName, message, novaMensagem.timestamp.ToString("HH:mm"));
    }
}
using dweb.Data;
using Microsoft.AspNetCore.SignalR;

namespace dweb.Controllers;

public class GostosHub : Hub
{
    private readonly AppDbContext _context;
    
    
    public GostosHub(AppDbContext context) {
        _context = context;
        
    }
    
    public async Task SendMessage(string message)
    {
        
    }
}
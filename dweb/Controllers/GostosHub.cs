using dweb.Data;
using Microsoft.AspNetCore.SignalR;

namespace dweb.Controllers;

public class GostosHub : Hub
{
    
    public async Task SendMessage(int movieId, int likes, int dislikes)
    {
        Clients.All.SendAsync("ReceiveMessage", movieId, likes, dislikes);
    }
}
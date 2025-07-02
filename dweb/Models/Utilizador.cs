using Microsoft.AspNetCore.Identity;

namespace dweb.Models;

public class Utilizador : IdentityUser
{
    public string? Imagem { get; set; } 
    public ICollection<Filme> ?Filmes { get; set; }
}
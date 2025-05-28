using Microsoft.AspNetCore.Identity;

namespace dweb.Models;

public class Utilizador : IdentityUser
{
    public ICollection<Filme> Filmes { get; set; }
}
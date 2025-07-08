using Microsoft.AspNetCore.Identity;

namespace dweb.Models;

public class Utilizador : IdentityUser
{
    public byte[]? Imagem { get; set; } 
    
    public ICollection<FilmeUtilizador>? FilmeUtilizador { get; set; }
    public ICollection<Mensagem> ?Mensagem { get; set; } 
    
   
}
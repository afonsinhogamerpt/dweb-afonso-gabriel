using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace dweb.Models;

public class Filme
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int filmeID { get; set; }
    public string nome { get; set; }
    public string resumo { get; set; }
    public string imagem { get; set; }
    public int ano { get; set; }
    
    public ICollection<Utilizador> Utilizador { get; set; } = new List<Utilizador>();
    public ICollection<Actor> Actor { get; set; } = new List<Actor>();
    public ICollection<Review> ?FKReview { get; set; }
    public ICollection<Genero> Genero { get; set; } = new List<Genero>();
    public ICollection<Director> Director { get; set; } = new List<Director>();
    public ICollection<Mensagem> ?Mensagem { get; set; } 

}
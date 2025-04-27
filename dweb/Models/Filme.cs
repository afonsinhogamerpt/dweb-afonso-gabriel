using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dweb.Models;

public class Filme
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int filmeID { get; set; }
    public string nome { get; set; }
    public string resumo { get; set; }
    public ICollection<Utilizador_Filme> FKUtilizador_Filme { get; set; } 
    public ICollection<Filme_Actor> FKFilme_Actor { get; set; } 
    public ICollection<Review> FKReview { get; set; }
}
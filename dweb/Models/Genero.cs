using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dweb.Models;

public class Genero
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int generoID { get; set; }
    public string nome { get; set; }
    public ICollection<Filme_Genero> FKFilme_Genero { get; set; }
}
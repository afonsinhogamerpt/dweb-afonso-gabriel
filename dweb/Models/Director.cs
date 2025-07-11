using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dweb.Models;

public class Director
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int directorID { get; set; }
    public string nome { get; set; }
    public int idade { get; set; }
    public string bio { get; set; }
    public string? imagem { get; set; }
    public ICollection<Filme> Filme { get; set; } = new List<Filme>();

}
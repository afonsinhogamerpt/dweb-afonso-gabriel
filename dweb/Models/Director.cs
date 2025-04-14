using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dweb.Models;

public class Director
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int directorID { get; set; }
    public string nome { get; set; }
    public ICollection<Filme_Director> FKFilme_Director { get; set; }
}
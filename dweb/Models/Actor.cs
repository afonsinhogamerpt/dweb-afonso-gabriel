using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dweb.Models;

public class Actor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int actorID { get; set; }
    [StringLength(100)]
    public string nome { get; set; }
    public int idade { get; set; }
    public string bio { get; set; }
    public ICollection<Filme_Actor> FKFilme_Actor { get; set; }
}
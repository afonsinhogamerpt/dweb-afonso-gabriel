using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dweb.Models;

public class Utilizador
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int utilizadorID { get; set; }
    [StringLength(25)]
    public string nome { get; set; }
    public string email { get; set; }
    public string apelido { get; set; }
    
    public ICollection<Utilizador_Filme> FKUtilizador_Filme { get; set; }
}
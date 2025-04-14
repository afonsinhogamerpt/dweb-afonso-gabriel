using System.ComponentModel.DataAnnotations.Schema;

namespace dweb.Models;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(utilizadorID), nameof(filmeID))]
public class Utilizador_Filme
{
    [ForeignKey(nameof(utilizadorID))]
    public int utilizadorID { get; set; }
    public Utilizador Utilizador { get; set; }
    
    [ForeignKey(nameof(filmeID))]
    public int filmeID { get; set; }
    
    public Filme Filme { get; set; }
}
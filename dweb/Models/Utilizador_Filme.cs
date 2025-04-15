using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace dweb.Models;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(utilizadorID), nameof(filmeID))]
public class Utilizador_Filme
{
    [ForeignKey(nameof(utilizadorID))]
    public string utilizadorID { get; set; }
    
    public IdentityUser Utilizador { get; set; }
    
    [ForeignKey(nameof(filmeID))]
    public int filmeID { get; set; }
    
    public Filme Filme { get; set; }
}
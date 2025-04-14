using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dweb.Models;

[PrimaryKey(nameof(filmeID), nameof(generoID))]
public class Filme_Genero
{
    [ForeignKey(nameof(filmeID))]
    public int filmeID { get; set; }
    public Filme filme { get; set; }
    
    [ForeignKey(nameof(generoID))]
    public int generoID { get; set; }
    public Genero genero { get; set; }
}
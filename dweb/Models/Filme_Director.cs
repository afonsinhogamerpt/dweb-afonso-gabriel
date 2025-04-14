using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dweb.Models;

[PrimaryKey(nameof(filmeID), nameof(directorID))]
public class Filme_Director
{
    [ForeignKey(nameof(filmeID))]
    public int filmeID { get; set; }
    public Filme filme { get; set; }
    
    [ForeignKey(nameof(directorID))]
    public int directorID { get; set; }
    public Director director { get; set; }
}
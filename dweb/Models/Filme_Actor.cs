using System.ComponentModel.DataAnnotations.Schema;

namespace dweb.Models;
using Microsoft.EntityFrameworkCore;


[PrimaryKey(nameof(filmeID), nameof(actorID))]
public class Filme_Actor
{
    [ForeignKey(nameof(filmeID))]
    public int filmeID { get; set; }
    public Filme filme { get; set; }
    
    [ForeignKey(nameof(actorID))]
    public int actorID { get; set; }
    public Actor actor { get; set; }
}
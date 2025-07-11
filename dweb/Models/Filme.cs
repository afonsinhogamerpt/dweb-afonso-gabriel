using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace dweb.Models;

public class Filme
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int filmeID { get; set; }
    public string nome { get; set; }
    public string resumo { get; set; }
    public string? imagem { get; set; }
    public int ano { get; set; }
    
    public int likes { get; set; }
    
    public int dislikes { get; set; }
    
    [JsonIgnore]
    public ICollection<FilmeUtilizador> FilmeUtilizador { get; set; }
    public ICollection<Actor> Actor { get; set; } = new List<Actor>();
    public ICollection<Genero> Genero { get; set; } = new List<Genero>();
    public ICollection<Director> Director { get; set; } = new List<Director>();
    

}
namespace dweb.Models.DTOs;

public class MovieDTO
{
    public string nome { get; set; }
    public string resumo { get; set; }
    public string imagem { get; set; }
    public int ano { get; set; }
    public List<int> Generos { get; set; }
    
    public List<int> Directores { get; set; }
    
    public List<int> Actores { get; set; }
    
}
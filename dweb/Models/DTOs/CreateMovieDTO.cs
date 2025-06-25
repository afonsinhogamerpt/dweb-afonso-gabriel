namespace dweb.Models.DTOs;

public class CreateMovieDTO
{
    public string nome { get; set; }
    public string resumo { get; set; }
    public string imagem { get; set; }
    public int ano { get; set; }
    
    public List<int> generos { get; set; }
    public List<int> directores { get; set; }
    
    public List<int> actores { get; set; }
}
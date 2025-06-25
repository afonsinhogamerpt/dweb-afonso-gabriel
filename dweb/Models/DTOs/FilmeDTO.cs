namespace dweb.Models.DTOs;

public class FilmeDTO
{
    public int filmeID { get; set; }
    public string nome { get; set; }
    public string resumo { get; set; }
    public string imagem { get; set; }
    public int ano { get; set; }
    public List<ActorDTO> actores { get; set; }
    public List<GeneroDTO> generos { get; set; }
    public List<DirectorDTO> directores { get; set; }
}
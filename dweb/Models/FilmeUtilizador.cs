namespace dweb.Models;

public class FilmeUtilizador
{
    public string UtilizadorId { get; set; }
    public Utilizador Utilizador { get; set; }

    public int FilmeId { get; set; }
    public Filme Filme { get; set; }

    public bool IsLike { get; set; }
}
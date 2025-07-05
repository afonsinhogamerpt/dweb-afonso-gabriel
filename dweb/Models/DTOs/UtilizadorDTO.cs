using System.Collections.Generic;

namespace dweb.Models.DTOs;

public class UtilizadorDTO
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? Imagem { get; set; }
    public List<FilmeDTO>? Filmes { get; set; }
}

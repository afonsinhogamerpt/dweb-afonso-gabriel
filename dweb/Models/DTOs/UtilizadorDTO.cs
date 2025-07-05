using System.Collections.Generic;

namespace dweb.Models.DTOs;

public class UtilizadorDTO
{
    public string Id { get; set; }
    public string Email { get; set; }
    public byte[]? Imagem { get; set; }
    public List<FilmeDTO>? Filmes { get; set; }
}

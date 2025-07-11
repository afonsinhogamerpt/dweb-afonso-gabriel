namespace dweb.Models.DTOs;

public class ActorDTO
{
    public int actorID { get; set; }
    public string nome { get; set; }
    public int idade { get; set; }
    public string bio { get; set; }
    public byte[]? imagem { get; set; }
}
namespace dweb.Models.DTOs;

public class DirectorDTO
{
    public int directorID { get; set; }
    public string nome { get; set; }
    public int idade { get; set; }
    public string bio { get; set; }
    public byte[]? imagem { get; set; }
}
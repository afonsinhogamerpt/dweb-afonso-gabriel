namespace dweb.Models.DTOs;

public class MensagemOutputDTO
{
    public int mensagemID { get; set; }
    public string conteudo { get; set; }
    public DateTime timestamp { get; set; }
    public string UserID { get; set; }
    public string username { get; set; }
}
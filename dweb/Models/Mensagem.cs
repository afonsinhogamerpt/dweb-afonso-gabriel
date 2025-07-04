using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NuGet.Packaging.Signing;

namespace dweb.Models;

public class Mensagem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int mensagemID { get; set; }
    public string conteudo { get; set; }
    
    [Required]
    public string UserID { get; set; }
    
    [ForeignKey(nameof(UserID))]
    public Utilizador? User { get; set; } 
    public DateTime timestamp { get; set; }
}
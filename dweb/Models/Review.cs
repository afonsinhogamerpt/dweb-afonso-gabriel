using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dweb.Models;

public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int reviewID { get; set; }
    [StringLength(1024)]
    public string conteudo { get; set; }
    [Required]
    public double rating { get; set; }
}
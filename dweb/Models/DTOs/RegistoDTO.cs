using System.ComponentModel.DataAnnotations;

namespace dweb.Models.DTOs;

public class RegistoDTO
{
    [Required(ErrorMessage = "É necessário utilizar um email para efetuar o registo")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "É necessário introduzir uma password")]
    public string Password { get; set; }
    
    public string ConfirmPassword { get; set; }
}
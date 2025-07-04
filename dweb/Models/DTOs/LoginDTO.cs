using System.ComponentModel.DataAnnotations;

namespace dweb.Models.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "É necessário introduzir o email")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "É necessário introduzir a password")]
    public string Password { get; set; }
    
    [Display(Name = "Lembrar?")]
    public bool RememberMe { get; set; }
}
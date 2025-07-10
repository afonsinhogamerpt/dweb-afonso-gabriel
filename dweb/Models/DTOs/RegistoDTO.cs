using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace dweb.Models.DTOs;

public class RegistoDTO
{
    [Required(ErrorMessage = "É necessário utilizar um email para efetuar o registo")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "É necessário introduzir uma password")]
    public string Password { get; set; }
    
    public string ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "É necessário introduzir um nome de utilizador para efetuar o registo")]
    public string UserName { get; set; }
}
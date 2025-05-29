using System.Runtime.InteropServices.JavaScript;
using dweb.Data;
using dweb.Models;
using dweb.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly AppDbContext _context;

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext context)
    {
        _userManager = userManager; 
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return Redirect("/Identity/Account/Login");
    }
    
    [HttpGet]
    public IActionResult Registo()
    {
        return Redirect("/Identity/Account/Register");
    }

    [HttpPost("registo")]
    public async Task<IActionResult> Registo([FromBody] RegistoDTO registo)
    {
        
        if (!ModelState.IsValid)
        {
            return View(registo);
        }
        
        var existingUser = await _userManager.FindByEmailAsync(registo.Email);
        if (existingUser != null)
        {
            return BadRequest("Já existe um utilizador com este email");
        }
        
        if (registo.Password != registo.ConfirmPassword)
        {
              return View(registo);
        }
        
        var u = new Utilizador { UserName = registo.Email, Email = registo.Email };
        var result = await _userManager.CreateAsync(u, registo.Password);

        if (result.Succeeded)
        {
            return Ok(new { message = "Utilizador registado com sucesso" });
        }

        var errors = result.Errors.Select(e => e.Description);
        return BadRequest(new { message = "Erro ao criar utilizador", erros = errors });
    }
    
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        if (!ModelState.IsValid)
        {
            return View(login);
        }
        
        var username = await _userManager.FindByEmailAsync(login.Email);
        if (username==null)
        {
            return Ok("Não existe nenhum utilizador registado com o email introduzido");
        }
        
        var result = await _signInManager.PasswordSignInAsync(username, login.Password, login.RememberMe, false);

        if (result.Succeeded)
        {
            return Redirect("/Home/Index");
        }
        
        return BadRequest("Não foi possível efetuar o login");
    }
}
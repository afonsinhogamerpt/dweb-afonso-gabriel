using System.Runtime.InteropServices.JavaScript;
using dweb.Data;
using dweb.Models;
using dweb.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;


public class AuthController : BaseController
{
    private readonly UserManager<Utilizador> _userManager;
    private readonly SignInManager<Utilizador> _signInManager;
    private readonly AppDbContext _context;
    
    public AuthController(UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager, AppDbContext context) : base(context)
    {
        _userManager = userManager; 
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return Redirect("/Identity/Account/Login");
    }
    
    [HttpGet("registo")]
    public IActionResult Registo()
    {
        return Redirect("/Identity/Account/Register");
    }

    [HttpPost("registo")]
    public async Task<IActionResult> Registo(RegistoDTO registo)
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
        
        var u = new Utilizador { UserName = registo.UserName, Email = registo.Email, NormalizedUserName = registo.UserName.ToUpper() };
        var result = await _userManager.CreateAsync(u, registo.Password);

        if (result.Succeeded && !User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }
        else
        {
            return Redirect("/User/Users");
        }

        var errors = result.Errors.Select(e => e.Description);
        return BadRequest(new { message = "Erro ao criar utilizador", erros = errors });
    }
    
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO login)
    {
        if (!ModelState.IsValid)
        {
            return View(login);
        }

        //return Ok(login);
        
        var username = await _userManager.FindByEmailAsync(login.Email);
        if (username==null)
        {
            return Ok("Não existe nenhum utilizador registado com o email introduzido");
        }
        
        var result = await _signInManager.PasswordSignInAsync(username, login.Password, login.RememberMe, false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }
        
        return BadRequest("Não foi possível efetuar o login");
    }
    
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return Redirect("/Identity/Account/Login");
    }
    
}
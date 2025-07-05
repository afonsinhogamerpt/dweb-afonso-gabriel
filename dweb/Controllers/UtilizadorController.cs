using dweb.Data;
using dweb.Models;
using dweb.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dweb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UtilizadorController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<Utilizador> _userManager;

    public UtilizadorController(AppDbContext context, UserManager<Utilizador> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UtilizadorDTO>>> GetUtilizadores()
    {
        var utilizadores = await _context.Utilizador
            .Include(u => u.Filmes)
            .Select(u => new UtilizadorDTO
            {
                Id = u.Id,
                Email = u.Email,
                Imagem = u.Imagem,
                Filmes = u.Filmes.Select(f => new FilmeDTO
                {
                    filmeID = f.filmeID,
                    nome = f.nome,
                    resumo = f.resumo,
                    imagem = f.imagem,
                    ano = f.ano
                }).ToList()
            })
            .ToListAsync();
        return Ok(utilizadores);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Utilizador>> GetUtilizador(string id)
    {
        var utilizador =  _context.Utilizador.Where(u => u.Id.Equals(id)). 
            FirstOrDefault();

        if (utilizador is not Utilizador)
        {
            return NotFound();
        }
        
        return Ok(utilizador);
    }
    
    [HttpPost("update-utilizador")]
    public async Task<IActionResult> UpdateDados([FromForm] Utilizador model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
        {
            return NotFound();
        }

        user.UserName = model.UserName;
        user.Email = model.Email;
        user.Imagem = model.Imagem;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            TempData["Error"] = "Erro ao atualizar os dados.";
            return RedirectToAction("Update", "User");
        }

        TempData["Success"] = "Dados atualizados com sucesso!";
        return RedirectToAction("Update", "User");
    }

    [HttpPut]
    public async Task<IActionResult> PutUtilizador(Utilizador utilizador)
    {
        var u = _context.Utilizador.
            Where(u => u.Id.Equals(utilizador.Id)).
            FirstOrDefault();

        if (u is not Utilizador)
        {
            return NotFound();
        }
        
        u.Email = utilizador.Email;
        await _context.SaveChangesAsync();
        return Ok(utilizador);
    }

    [HttpPost]
    public async Task<IActionResult> PostUtilizador(Utilizador utilizador)
    {
        
        _context.Utilizador.Add(utilizador);
        await _context.SaveChangesAsync();
        return Ok(utilizador);
    }

    [HttpPost("delete-utilizador")]
    public async Task<IActionResult> DeleteUtilizador([FromForm] string id)
    {
        var u = _context.Utilizador. 
            Where(u => u.Id.Equals(id)). 
            FirstOrDefault();

        if (u == null)
        {
            return NotFound();
        }
        
        _context.Utilizador.Remove(u);
        await _context.SaveChangesAsync();
        return Redirect("/Identity/Account/Login");
    }
    
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromForm] string id, [FromForm] string newPassword)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded)
        {
            TempData["Error"] = "Erro ao alterar a password.";
            return BadRequest();
        }

        TempData["Success"] = "Password alterada com sucesso.";
        return Redirect("/Identity/Account/Login");
    } 
    
}
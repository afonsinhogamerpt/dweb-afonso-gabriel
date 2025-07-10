using System.Security.Claims;
using dweb.Data;
using dweb.Models;
using dweb.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dweb.Controllers;

public class LikeController : BaseController
{
    
    private readonly AppDbContext _context;
    private readonly UserManager<Utilizador> _userManager;

    public LikeController(AppDbContext context, UserManager<Utilizador> userManager) : base(context)
    {
        _context = context;
        _userManager = userManager;
    }
   
    [HttpPost("Like")]
    public async Task<ActionResult> Like(LikeDTO like)
    {
        var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userID == null)
        {
            return Redirect("/Identity/Account/Login");
        }
        var filme = await _context.Filme.FirstOrDefaultAsync(f => f.filmeID == like.filmeID);
        if (filme == null)
        {
            return NotFound();
        }
        var reacao = await _context.FilmeUtilizador
            .FirstOrDefaultAsync(fu => fu.FilmeId == filme.filmeID && fu.UtilizadorId == userID);

        if (reacao != null)
        {
            if (reacao.IsLike)
            {
                filme.likes--;
                _context.FilmeUtilizador.Remove(reacao);
            }
            else
            {
                filme.likes++;
                filme.dislikes--;
                reacao.IsLike = true;
            }
        }
        else
        {
            filme.likes++;
            var novaReacao = new FilmeUtilizador
            {
                FilmeId = filme.filmeID,
                UtilizadorId = userID,
                IsLike = true
            };
            _context.FilmeUtilizador.Add(novaReacao);
        }

        await _context.SaveChangesAsync();

        return Redirect($"/Filme/FilmeDetails/{filme.filmeID}");
    }
    
    
    [HttpPost("Dislike")]
    public async Task<ActionResult> Dislike(LikeDTO like)
    {
        var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userID == null)
        {
            return Redirect("/Identity/Account/Login");
        }

        var filme = await _context.Filme.FirstOrDefaultAsync(f => f.filmeID == like.filmeID);
        if (filme == null)
        {
            return NotFound();
        }
        
        var reacao = await _context.FilmeUtilizador
            .FirstOrDefaultAsync(fu => fu.FilmeId == filme.filmeID && fu.UtilizadorId == userID);

        if (reacao != null)
        {
            if (reacao.IsLike == false)
            {
                filme.dislikes--;
                _context.FilmeUtilizador.Remove(reacao);
            }
            else
            {
                filme.likes--;
                filme.dislikes++;
                reacao.IsLike = false;
            }
        }
        else
        {
            filme.dislikes++;
            var novaReacao = new FilmeUtilizador
            {
                FilmeId = filme.filmeID,
                UtilizadorId = userID,
                IsLike = false
            };
            _context.FilmeUtilizador.Add(novaReacao);
        }

        await _context.SaveChangesAsync();

        return Redirect($"/Filme/FilmeDetails/{filme.filmeID}");
    }
}
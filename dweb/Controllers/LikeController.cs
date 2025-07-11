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
   
    [HttpPost]
    public async Task<ActionResult> Like(LikeDTO like)
    {
        var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userID == null) return Redirect("/Identity/Account/Login");

        var filme = await _context.Filme.FindAsync(like.filmeID);
        if (filme == null) return NotFound();

        var reacao = await _context.FilmeUtilizador
            .FirstOrDefaultAsync(fu => fu.FilmeId == like.filmeID && fu.UtilizadorId == userID);

        if (reacao == null)
        {
            filme.likes++;
            _context.FilmeUtilizador.Add(new FilmeUtilizador
            {
                FilmeId = like.filmeID,
                UtilizadorId = userID,
                IsLike = true
            });
        }
        else
        {
            switch (reacao.IsLike)
            {
                case true:
                    filme.likes--;
                    reacao.IsLike = null;
                    break;
                case false:
                    filme.dislikes--;
                    filme.likes++;
                    reacao.IsLike = true;
                    break;
                case null:
                    filme.likes++;
                    reacao.IsLike = true;
                    break;
            }
            _context.FilmeUtilizador.Update(reacao);
        }

        await _context.SaveChangesAsync();
        return Redirect($"/Filme/FilmeDetails/{filme.filmeID}");
    }
    
    
    [HttpPost]
    public async Task<ActionResult> Dislike(LikeDTO like)
    {
        var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userID == null) return Redirect("/Identity/Account/Login");

        var filme = await _context.Filme.FindAsync(like.filmeID);
        if (filme == null) return NotFound();

        var reacao = await _context.FilmeUtilizador
            .FirstOrDefaultAsync(fu => fu.FilmeId == like.filmeID && fu.UtilizadorId == userID);

        if (reacao == null)
        {
            filme.dislikes++;
            _context.FilmeUtilizador.Add(new FilmeUtilizador
            {
                FilmeId = like.filmeID,
                UtilizadorId = userID,
                IsLike = false
            });
        }
        else
        {
            switch (reacao.IsLike)
            {
                case false:
                    filme.dislikes--;
                    reacao.IsLike = null;
                    break;
                case true:
                    filme.likes--;
                    filme.dislikes++;
                    reacao.IsLike = false;
                    break;
                case null:
                    filme.dislikes++;
                    reacao.IsLike = false;
                    break;
            }
            _context.FilmeUtilizador.Update(reacao);
        }

        await _context.SaveChangesAsync();
        return Redirect($"/Filme/FilmeDetails/{filme.filmeID}");
    }
}
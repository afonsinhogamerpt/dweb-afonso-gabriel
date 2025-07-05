using System.Security.Claims;
using dweb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dweb.Controllers;

public class BaseController : Controller
{
    protected readonly AppDbContext _context;

    public BaseController(AppDbContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId))
        {
            var user = _context.Utilizador.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                ViewData["UserImage"] = string.IsNullOrEmpty(user.Imagem)
                    ? null
                    : user.Imagem;
                ViewData["UserId"] = user.Id;
            }
        }

        base.OnActionExecuting(context);
    }
}
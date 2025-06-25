using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

public class ChatController : Controller
{
    // GET
    [Authorize("administrador")]
    [Route("chat")]
    public IActionResult Chat()
    {
        return View();
    }
}
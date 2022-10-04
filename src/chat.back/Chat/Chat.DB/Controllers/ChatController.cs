using Microsoft.AspNetCore.Mvc;

namespace Chat.DB.Controllers;

[ApiController]
[Route("api/chat")]
[Produces("application/json")]
public class MessageController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetChatHistory(string room)
    {
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveMessage(string message)
    {
        return Ok();
    }
}
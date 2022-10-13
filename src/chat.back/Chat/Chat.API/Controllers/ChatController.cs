using Chat.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("api/messages")]
[Produces("application/json")]
public class MessageController : ControllerBase
{
    private readonly IChatRepository _chatRepository;

    public MessageController(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetChatHistory(string room)
    {
        var chatHistory = await _chatRepository.GetChatMessages(room);
        return Ok(
            chatHistory.Where(r => r.Room == room)
            .Select(s => new {s.User, s.Message}));
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveMessage(string room, string user, string message)
    {
        var res =  await _chatRepository.SaveMessage(room, user, message);
        return res ? Ok() : BadRequest();
    }
}
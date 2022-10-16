
namespace Chat.Domain.Entities;

public class ChatMessage
{
    public int Id { get; set; }
    public string Room { get; set; } = null!;
    public string User { get; set; } = null!;

    public string Message { get; set; } = null!;

}
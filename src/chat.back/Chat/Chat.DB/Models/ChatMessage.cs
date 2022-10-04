using System.ComponentModel.DataAnnotations;

namespace Chat.DB.Models;

public class ChatMessage
{
    [Key]
    public string Room { get; set; } = null!;
    [Key] 
    public string User { get; set; } = null!;

    public string Message { get; set; } = null!;

}
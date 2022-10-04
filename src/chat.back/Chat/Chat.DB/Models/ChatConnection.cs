using System.ComponentModel.DataAnnotations;

namespace Chat.DB.Models;

public class ChatConnection
{
    [Key]
    public string ConnectionId { get; set; } = null!;
    
    public string User { get; set; } = null!;
    public string Room { get; set; } = null!;
}
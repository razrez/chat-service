using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Entities;

public class ChatConnection
{
    [Key]
    public string ConnectionId { get; set; } = null!;
    
    public string Room { get; set; } = null!;
    public string User { get; set; } = null!;
}
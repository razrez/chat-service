using Chat.DB.Models;
using ChatConnection = Chat.Domain.Entities.ChatConnection;
using ChatMessage = Chat.Domain.Entities.ChatMessage;

namespace Chat.Infrastructure.Persistence.Repository;

public interface IChatRepository
{
    // пока не нужно, заменено на дикшнари
    Task<List<ChatConnection>> GetChatConnections(string room);
    Task<List<ChatMessage>> GetChatMessages(string room);
    
    Task<bool> SaveMessage(string room, string user, string message);
    Task<bool> SaveConnection(string connectionId, string room, string user);
    
}
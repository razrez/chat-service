using Chat.DB.Models;

namespace Chat.DB.Data.Repository;

public interface IChatRepository
{
    // пока не нужно, заменено на дикшнари
    Task<IEnumerable<ChatConnection>> GetChatConnections(string room);
    Task<IEnumerable<ChatMessage>> GetChatMessages(string room);
    
    Task<bool> SaveMessage(string room, string user, string message);
    Task<bool> SaveConnection(string connectionId, string room, string user);
    
}
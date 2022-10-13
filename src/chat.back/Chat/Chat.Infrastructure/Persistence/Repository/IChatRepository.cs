using Chat.Domain.Entities;

namespace Chat.Infrastructure.Persistence.Repository;

public interface IChatRepository
{
    Task<List<ChatMessage>> GetChatMessages(string room);
    
    Task<bool> SaveMessage(string room, string user, string message);
    
}
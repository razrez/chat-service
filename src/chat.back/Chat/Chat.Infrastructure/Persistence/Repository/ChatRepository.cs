using Microsoft.EntityFrameworkCore;
using Chat.Domain.Entities;

namespace Chat.Infrastructure.Persistence.Repository;

public class ChatRepository : IChatRepository
{
    private readonly ApplicationDbContext _chatContext;

    public ChatRepository(ApplicationDbContext chatContext)
    {
        _chatContext = chatContext;
    }
    
    //list of connected users to the room
    public async Task<List<ChatConnection>> GetChatConnections(string room)
    {
        return await _chatContext.ChatConnections.ToListAsync();
    }
    
    //current room's chat history
    public async Task<List<ChatMessage>> GetChatMessages(string room)
    {
        return await _chatContext.ChatMessages.ToListAsync();
    }
    
    public async Task<bool> SaveMessage(string room, string user, string message)
    {
        try
        {
            await _chatContext.ChatMessages
                .AddAsync(new ChatMessage()
                {
                    Room = room, 
                    User = user,
                    Message = message
                });
            
            var saveRes = await _chatContext.SaveChangesAsync();
            return saveRes > 0; // > 0 => saved
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SaveConnection(string connectionId, string room, string user)
    {
        try
        {
            await _chatContext.ChatConnections
                .AddAsync(new ChatConnection()
                {
                    ConnectionId = connectionId,
                    Room = room, 
                    User = user
                });
            
            var saveRes = await _chatContext.SaveChangesAsync();
            return saveRes > 0; // > 0 => saved
        }
        catch (Exception)
        {
            return false;
        }
    }
}
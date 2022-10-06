using Chat.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.DB.Data.Repository;

public class ChatRepository : IChatRepository
{
    private readonly ChatContext _chatContext;

    public ChatRepository(ChatContext chatContext)
    {
        _chatContext = chatContext;
    }
    
    //list of connected users to the room
    public async Task<IEnumerable<ChatConnection>> GetChatConnections(string room)
    {
        return await _chatContext.ChatConnections.ToListAsync();
    }
    
    //current room's chat history
    public async Task<IEnumerable<ChatMessage>> GetChatMessages(string room)
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
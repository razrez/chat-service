using System.Collections.Concurrent;
using Grpc.Core;

namespace Chat.AppCore.Services.ChatGRPC;

public class ChatRoom
{
    // key - roomName - userName, value - messages
    private readonly ConcurrentDictionary<string, IServerStreamWriter<Message>> _users = new();
    public void Join(string name, IServerStreamWriter<Message> response) => _users.TryAdd(name, response);

    public void Remove(string name)  => _users.TryRemove(name, out _);

    public async Task BroadcastMessageAsync(Message message) => await BroadcastMessages(message);

    private async Task BroadcastMessages(Message message)
    {
        foreach (var user in _users.Where(x => x.Key != message.Sender.Username))
        {
            var item = await SendMessageToSubscriber(user, message);
            if (item != null)
            {
                Remove(item?.Key!);
            }
        }
    }

    private async Task<KeyValuePair<string, IServerStreamWriter<Message>>?> SendMessageToSubscriber(KeyValuePair<string, IServerStreamWriter<Message>> user, Message message)
    {
        try
        {
            await user.Value.WriteAsync(message);
            return null;
        }
        
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return user;
        }
    }
}
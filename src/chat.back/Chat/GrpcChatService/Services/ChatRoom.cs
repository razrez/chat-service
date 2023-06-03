using System.Collections.Concurrent;
using Chat;
using Chat.API.Hubs.Models;
using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace GrpcChatService.Services;

public class ChatRoom
{
    // name - the key
    private readonly IDictionary<string, UserConnection> _connections;

    private readonly HubConnection hubClient = new HubConnectionBuilder()
        .WithUrl("http://localhost:5038/chat")
        .Build();
    
    // users' responses 
    private readonly ConcurrentDictionary<string, IServerStreamWriter<Message>> _userResponses = new();

    public ChatRoom(IDictionary<string, UserConnection> connections)
    {
        _connections = connections;
        hubClient.StartAsync();
    }

    public async Task Join(Message userMessage, IServerStreamWriter<Message> response)
    {
        _userResponses.TryAdd(userMessage.User, response);
        
        var userConnection =  new UserConnection
        {
            User = userMessage.User,
            Room = userMessage.Room
        };
        
        _connections.TryAdd(userMessage.User, userConnection);
        
        // for mobile client
        if (!userMessage.User.Contains("admin"))
        {
            await hubClient.InvokeAsync("JoinRoom", userConnection);
        }
        
    }
    
    public void Remove(string name)
    {
        _userResponses.TryRemove(name, out var s);
        _connections.Remove(name);
    }

    public async Task BroadcastMessageAsync(Message message) => await BroadcastMessages(message);

    private async Task BroadcastMessages(Message message)
    {
        // send message to others
        foreach (var user in _userResponses.Where(x => x.Key != message.User))
        {
            // check if user (receiver) isn't in the same room with message sender
            _connections.TryGetValue(user.Key, out var receiver);
            if (receiver != null & message.Room != receiver.Room) continue;
            
            // send message from MOBILE CLIENT to admin in web through SignalR
            if (receiver.User.Contains("admin"))
            {
                await hubClient.InvokeAsync("SendMessageFromMobile", message.User, message.Room, message.Text);
                
                continue;
            };
            
            // sender - ADMIN, receiver - mobile user with gRPC connection
            var item = await SendMessageToSubscriber(user, message);
            if (item != null)
            {
                Remove(item?.Key!);
            };

        }
    }

    // returns null if message successfully sent 
    private async Task<Nullable<KeyValuePair<string, IServerStreamWriter<Message>>>> SendMessageToSubscriber(KeyValuePair<string, IServerStreamWriter<Message>> user, Message message)
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
using Chat.API.Hubs.Models;
using Chat.API.Publisher;
using Chat.AppCore.Common.DTO;
using Microsoft.AspNetCore.SignalR;

namespace Chat.API.Hubs;

public class ChatHub : Hub
{
    private readonly IDictionary<string, UserConnection> _connections;
    private readonly IMessagePublisher _publisher;

    public ChatHub(IDictionary<string, UserConnection> connections, IMessagePublisher publisher)
    {
        _connections = connections;
        _publisher = publisher;
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public async Task JoinRoom(UserConnection userConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
        _connections[Context.ConnectionId] = userConnection;
        
        //real-time уведомление от Бота
        await Clients.Group(userConnection.Room)
            .SendAsync(
                "ReceiveMessage","ChatBot",
                $"{userConnection.User} has joined {userConnection.Room}"
                );
        
        await SendConnectedUsers(userConnection.Room);
    }

    public async Task SendMessage(string message)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            await Clients.Group(userConnection.Room)
                .SendAsync("ReceiveMessage", userConnection.User, message);
            
            //тут должна быть логика для передачи сообщения в MassTransit, который потом добавляет сообщение в бд
            _publisher.SaveMessage(new SaveMessageDto(
                User: userConnection.User, 
                Room: userConnection.Room, 
                Message: message));
        }
    }
    
    public async Task SendMetadata(MetadataDto? metadataDto)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            await Clients.Group(userConnection.Room)
                .SendAsync("ReceiveMeta", metadataDto);
            
            //далее meta отправляются в rabbit на сохранение в монгодб
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            _connections.Remove(Context.ConnectionId);
            Clients.Group(userConnection.Room)
                .SendAsync("ReceiveMessage", "ChatBot", $"{userConnection.User} has left the room");

            SendConnectedUsers(userConnection.Room);
        }
        return base.OnDisconnectedAsync(exception);
    }

    public Task SendConnectedUsers(string room)
    {
        var users = _connections.Values
            .Where(c => c.Room == room)
            .Select(c => c.User);

        return Clients.Group(room).SendAsync("UsersInRoom", users);
    }
}
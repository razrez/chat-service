using Chat.API.Hubs.Models;
using Chat.AppCore.Common.DTO;
using Chat.AppCore.Publisher;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.SignalR;

namespace Chat.API.Hubs;

public class ChatHub : Hub
{
    private readonly IDictionary<string, UserConnection> _connections;
    private readonly IMessagePublisher _publisher;
    private readonly UsersQueue _usersQueue;
    
    private readonly AsyncDuplexStreamingCall<Message, Message> _grpcChatClientConnection ;
    
    public ChatHub(IDictionary<string, UserConnection> connections, IMessagePublisher publisher, UsersQueue usersQueue)
    {
        _connections = connections;
        _publisher = publisher;
        _usersQueue = usersQueue;

        #region setting gRPC-chat Client
        
        var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
        var channel = GrpcChannel.ForAddress("http://localhost:5059", new GrpcChannelOptions
        {
            HttpClient = new HttpClient(handler)
        });
        var client = new ChatRoom.ChatRoomClient(channel);
        _grpcChatClientConnection = client.join();
        
        #endregion
    }

    public async Task JoinRoom(UserConnection userConnection)
    {
        // add user to the queue
        _usersQueue.AddUser(userConnection.User);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
        _connections[Context.ConnectionId] = userConnection;
        
        //real-time уведомление от Бота
        await Clients.Group(userConnection.Room)
            .SendAsync(
                "ReceiveMessage","ChatBot",
                $"{userConnection.User} has joined"
                );
        
        await SendConnectedUsers(userConnection.Room);
    }
    
    public async Task JoinRoomByAdmin(string adminName)
    {
        var roomToHelp = _usersQueue.HelpUser();
        
        if (roomToHelp != "")
        {
            // define room for admin
            await Clients.Client(Context.ConnectionId).SendAsync("AdminInfo", roomToHelp);
            
            var adminConnection = new UserConnection
            {
                User = adminName,
                Room = roomToHelp
            };
            
            Console.WriteLine(adminConnection.Room);
            
            await Groups.AddToGroupAsync(Context.ConnectionId, adminConnection.Room);
            _connections[Context.ConnectionId] = adminConnection;
            
        
            //real-time message from ChatBot
            await Clients.Group(adminConnection.Room)
                .SendAsync(
                    "ReceiveMessage","ChatBot",
                    $"{adminConnection.User} has joined"
                );

            await SendConnectedUsers(adminConnection.Room);
            
            // grpc connection with client who uses mobile version
            await JoinMessageToGrpc(adminConnection.User, adminConnection.Room);

        }
        
        else
        {
            await Clients.Client(Context.ConnectionId).SendAsync("AdminInfo", "");
        }
    }

    public async Task SendMessage(string message)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            await Clients.Group(userConnection.Room)
                .SendAsync("ReceiveMessage", userConnection.User, message);
            
            // invoke gRPC method
            await SendMessageToGrpc(userConnection.User, message, userConnection.Room);
            
            // логика для передачи сообщения в очередь Rabbit'а, который потом добавляет сообщение в бд
            /*_publisher.SaveMessage(new SaveMessageDto(
                User: userConnection.User, 
                Room: userConnection.Room, 
                Message: message));*/
        }
    }

    public async Task SendMessageFromMobile(string user, string room, string message)
    {
        
        await Clients.Group(room)
            .SendAsync("ReceiveMessage", user, message);
        
        // логика для передачи сообщения в очередь Rabbit'а, который потом добавляет сообщение в бд
        /*_publisher.SaveMessage(new SaveMessageDto(
            User: user, 
            Room: room, 
            Message: message));*/
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

    private async Task JoinMessageToGrpc(string username, string room)
    {
        
        // отправка остальным уведомления о том, что ты присоединился 
        await _grpcChatClientConnection
            .RequestStream
            .WriteAsync(new Message 
            {
                User = username, Text = $"Hello, hope I'll help you!", Room = room
            });
        
        //await chat.RequestStream.CompleteAsync();
        //await channel.ShutdownAsync();
    }
    
    private async Task SendMessageToGrpc(string username, string message, string room)
    {
        
        // отправка сообщения
        await _grpcChatClientConnection
            .RequestStream
            .WriteAsync(new Message
            {
                User = username, Text = message, Room = room
            });
        
        //await chat.RequestStream.CompleteAsync();
        //await channel.ShutdownAsync();
    }
    
}
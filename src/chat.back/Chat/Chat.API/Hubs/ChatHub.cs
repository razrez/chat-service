using Amazon.S3;
using Chat.API.Hubs.Models;
using Chat.API.Publisher;
using Chat.AppCore.Common.DTO;
using Microsoft.AspNetCore.SignalR;
using PutObjectRequest = Amazon.S3.Model.PutObjectRequest;

namespace Chat.API.Hubs;

public class ChatHub : Hub
{
    private readonly IDictionary<string, UserConnection> _connections;
    private readonly IMessagePublisher _publisher;
    private readonly IAmazonS3 _s3Client;

    public ChatHub(IDictionary<string, UserConnection> connections, IMessagePublisher publisher, IAmazonS3 s3Client)
    {
        _connections = connections;
        _publisher = publisher;
        _s3Client = s3Client;
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
    
    public async Task SendFile(IFormFile file)
    {
        if (_connections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            /*await Clients.Group(userConnection.Room)
                .SendAsync("ReceiveFile", userConnection.User, file);*/

            var bucketName = userConnection.Room;
            var prefix = userConnection.User;
            
            //тут должна быть логика для передачи сообщения в MassTransit, который потом добавляет сообщение в бд
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix.TrimEnd('/')}/{file.FileName}",
                InputStream = file.OpenReadStream()
            };
            request.CannedACL = S3CannedACL.PublicRead;

            request.Metadata.Add("Name", file.FileName);
            request.Metadata.Add("ContentType", file.Headers.ContentType);
            request.Metadata.Add("Room", bucketName);
            request.Metadata.Add("User", prefix);
            
            await _s3Client.PutObjectAsync(request);
            
            var meta = new MetadataDto(
                FileName:file.FileName, 
                ContentType:file.ContentType, 
                RoomName:bucketName, 
                User:prefix!
            );
            
            await Clients.Group(userConnection.Room)
                .SendAsync("ReceiveFile", userConnection.User, meta);
            
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
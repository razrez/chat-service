﻿using System.Collections.Concurrent;
using Chat;
using Chat.API.Hubs.Models;
using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace GrpcChatService.Services;

public class ChatRoom
{
    private readonly IDictionary<string, UserConnection> _connections;

    private HubConnection hubClient = new HubConnectionBuilder()
        .WithUrl("http://localhost:5038/chat")
        .Build();
    
    // users' responses 
    private ConcurrentDictionary<string, IServerStreamWriter<Message>> userResponses = new();

    public ChatRoom(IDictionary<string, UserConnection> connections)
    {
        _connections = connections;
    }

    public async void Join(Message userMessage, IServerStreamWriter<Message> response)
    {
        userResponses.TryAdd(userMessage.User, response);
        
        var userConnection =  new UserConnection
        {
            User = userMessage.User,
            Room = userMessage.Room
        };
        
        _connections.TryAdd(userMessage.User, userConnection);

        if (userMessage.User == userMessage.Room)
        {
            await hubClient.StartAsync();
            await hubClient.InvokeAsync("JoinRoom", userConnection);
        }

    }

    public void Remove(string name)
    {
        userResponses.TryRemove(name, out var s);
    }

    public async Task BroadcastMessageAsync(Message message) => await BroadcastMessages(message);

    private async Task BroadcastMessages(Message message)
    {
        // send message to others
        foreach (var user in userResponses.Where(x => x.Key != message.User))
        {
            // check if user (receiver) isn't in the same room with message sender
            _connections.TryGetValue(user.Key, out var receiver);
            if (message.Room != receiver.Room) continue;
            
            // send message from mobile client to admin in web through SignalR
            if (message.Room == message.User)
            {
                await hubClient.InvokeAsync("SendMessageFromMobile", message.User, message.Room, message.Text);
                
                continue;
            };
            
            // if receiver - mobile user with gRPC connection
            var item = await SendMessageToSubscriber(user, message);
            if (item != null)
            {
                Remove(item?.Key!);
            };

        }
    }

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
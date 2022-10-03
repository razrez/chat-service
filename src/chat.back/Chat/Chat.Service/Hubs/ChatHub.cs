using Chat.Service.Hubs.Models;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Service.Hubs;

public class ChatHub : Hub
{
    public async Task JoinRoom(UserConnection userConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
        await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", $"{userConnection.User} has joined {userConnection.Room}");
    }

    public async Task SendMessage()
    {
        
    }

}
// See https://aka.ms/new-console-template for more information

using Chat;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

Console.WriteLine("Enter your name: ");
var userName = Console.ReadLine();

var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
var channel = GrpcChannel.ForAddress("http://localhost:5059", new GrpcChannelOptions
{
    HttpClient = new HttpClient(handler)
});
        
ChatRoom.ChatRoomClient client = new(channel);
using var chat = client.join();

// отправка остальным уведомления о том, что ты присоединился 
await chat.RequestStream.WriteAsync(new Message { User = userName, Text = $"{userName} has joined the room" });

await chat.RequestStream.CompleteAsync();
await channel.ShutdownAsync();
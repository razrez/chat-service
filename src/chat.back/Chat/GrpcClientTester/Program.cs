// See https://aka.ms/new-console-template for more information

using Chat;
using Grpc.Net.Client;

Console.WriteLine("Enter your name: ");
var userName = Console.ReadLine();

Console.WriteLine("name accepted");
using var channel = GrpcChannel.ForAddress("http://localhost:5059");
var client = new ChatRoom.ChatRoomClient(channel);

using (var chat = client.join())
{
    // вывод получаемых сообщений
    _ = Task.Run(async () =>
    {
        while (await chat.ResponseStream.MoveNext(cancellationToken: CancellationToken.None))
        {
            var response = chat.ResponseStream.Current;
            Console.WriteLine($"{response.User}: {response.Text}");
        }
    });
    
    // отправка остальным уведомления о том, что ты присоединился 
    await chat.RequestStream.WriteAsync(new Message
    {
        User = userName, 
        Text = $"{userName} has joined the room", 
        Room = userName
    });
    
    // отправка сообщения / выход из чата при вводе "bye"
    string? line;
    while ((line = Console.ReadLine()) != null)
    {
        if (line.ToLower() == "bye")
        {
            break;
        }
        await chat.RequestStream.WriteAsync(new Message { User = userName, Text = line, Room = userName});
    }
    await chat.RequestStream.CompleteAsync();
}

Console.WriteLine("Disconnecting");
await channel.ShutdownAsync();
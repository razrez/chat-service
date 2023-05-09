// See https://aka.ms/new-console-template for more information

using Chat.AppCore;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5167");
var client = new GrpcClientTester.Chat.ChatClient(channel);
// get own message
// var reply = await client.ChatMessaging();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
Console.WriteLine("Hello, World!");
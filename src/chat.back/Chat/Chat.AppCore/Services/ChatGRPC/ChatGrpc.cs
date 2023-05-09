using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Chat.AppCore.Services.ChatGRPC;

public class ChatGrpc : Chat.ChatBase
{
    private readonly ILogger<ChatGrpc> _logger;
    private readonly ChatRoom _chatroomService;

    public ChatGrpc(ChatRoom chatroomService, ILogger<ChatGrpc> logger)
    {
        _chatroomService = chatroomService;
        _logger = logger;
    }

    public override async Task ChatMessaging(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
    {
        if (!await requestStream.MoveNext()) return;
        
        // chatting
        do
        {
            // add message
            _chatroomService.Join(requestStream.Current.Sender.Username, responseStream);
            
            // send message to everyone
            await _chatroomService.BroadcastMessageAsync(requestStream.Current);
        } 
        while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested);
        
        // leave the chat
        _chatroomService.Remove(context.Peer);
    }
}
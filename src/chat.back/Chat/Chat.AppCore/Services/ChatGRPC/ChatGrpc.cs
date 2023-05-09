using Grpc.Core;

namespace Chat.AppCore.Services.ChatGRPC;

public class ChatGrpc : Chat.ChatBase
{
    public override async Task ChatMessaging(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
    {
        if (!await requestStream.MoveNext() ) return;

        do
        {
            

        }
        while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested);
        //return base.ChatMessaging(requestStream, responseStream, context);
    }
}
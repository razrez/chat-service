using Chat.API.Hubs;
using Chat.AppCore.Common.DTO;
using Chat.AppCore.Services.CacheService;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace Chat.API.Consumer;

public class RedisSubscriber : BackgroundService
{
    private const string ChannelName = "sync";
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ICacheService _cacheService;
    private readonly IHubContext<ChatHub> _hub;
    private MetadataDto? _metadataDto;
    private string? _fileId;
    
    public RedisSubscriber(IConnectionMultiplexer connectionMultiplexer, IHubContext<ChatHub> hub,
        ICacheService cacheService)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _hub = hub;
        _cacheService = cacheService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var sub = _connectionMultiplexer.GetSubscriber();
        await sub.SubscribeAsync(ChannelName, (channel, value) =>
        {
            var recordCounter = _connectionMultiplexer.GetDatabase().StringGet(value.ToString());
            //var recordCounter = await _cacheService.GetRecordAsync<string>(value!);
            Console.WriteLine(recordCounter);
            if (recordCounter == "2")
            {
                _metadataDto = _cacheService.GetRecord<MetadataDto>("Metadata_" + value);
                _fileId = _connectionMultiplexer.GetDatabase().StringGet("FileId_" + value);
                _hub.Clients.Group(_metadataDto!.RoomName)
                    .SendAsync("ReceiveMeta", _metadataDto, cancellationToken: stoppingToken);
            }
        });

        await Task.CompletedTask;
    }
}
using Chat.API.Hubs;
using Chat.AppCore.Common.DTO;
using Chat.AppCore.Services;
using Chat.AppCore.Services.CacheService;
using Chat.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace Chat.API.Consumer;

public class RedisSubscriber : BackgroundService
{
    private const string ChannelName = "sync";
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ICacheService _cacheService;
    private readonly IHubContext<ChatHub> _hub;
    private readonly MetadataService _metadata;
    private MetadataDto? _metadataDto;
    private string? _fileId;
    
    public RedisSubscriber(IConnectionMultiplexer connectionMultiplexer, IHubContext<ChatHub> hub,
        ICacheService cacheService, MetadataService metadata)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _hub = hub;
        _cacheService = cacheService;
        _metadata = metadata;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var sub = _connectionMultiplexer.GetSubscriber();
        await sub.SubscribeAsync(ChannelName,  (_, value) =>
        {
            var recordCounter = _connectionMultiplexer.GetDatabase().StringGet(value.ToString());
            Console.WriteLine(recordCounter);
            if (recordCounter == "2")
            {
                _metadataDto = _cacheService.GetRecord<MetadataDto>("Metadata_" + value);
                _fileId = _connectionMultiplexer.GetDatabase().StringGet("FileId_" + value);
                _hub.Clients.Group(_metadataDto!.RoomName)
                    .SendAsync("ReceiveMeta", _metadataDto, cancellationToken: stoppingToken);
                
                _metadata.Create(new MetadataFile
                {
                    FileName = _metadataDto.FileName,
                    ContentType = _metadataDto.ContentType,
                    RoomName = _metadataDto.RoomName,
                    User = _metadataDto.User
                });
            }
        });

        await Task.CompletedTask;
    }
}
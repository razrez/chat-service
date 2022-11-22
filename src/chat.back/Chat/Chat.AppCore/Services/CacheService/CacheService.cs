using System.Text.Json;
using Chat.AppCore.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Chat.AppCore.Services.CacheService;

public class CacheService : ICacheService
{
    private IDatabase _db;
    private readonly IDistributedCache _distributedCache;
    
    public CacheService(IConnectionMultiplexer connectionMultiplexer, IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _db = connectionMultiplexer.GetDatabase();
    }
    
    public async Task SetRecordAsync<T>(string key, T data, TimeSpan? expireTime = null)
    {
        var jsonData =  JsonSerializer.Serialize(data);
        await _db.StringSetAsync(
                key:key, 
                value:jsonData, 
                expiry: expireTime?? TimeSpan.FromSeconds(180))
            .ConfigureAwait(false);
    }

    public async Task<T?> GetRecordAsync<T>(string key)
    {
        return await _distributedCache.GetRecordAsync<T>(key);
    }

    public T? GetRecord<T>(string key)
    {
        var dataString = _db.StringGet(key);
        return JsonSerializer.Deserialize<T>(dataString!);
    }

    public async Task IncrementAsync(string key)
    {
        await _db.StringIncrementAsync(key);
    }

    public async Task AppendRecordAsync<T>(string key, T data, TimeSpan? expireTime = null)
    {
        var jsonData =  JsonSerializer.Serialize(data);
        await _db.StringAppendAsync(key, jsonData);
        await _db.StringGetSetExpiryAsync(key, expireTime?? TimeSpan.FromSeconds(180));
    }
    
    // синхронизация запросов
    public async Task SyncRequest(string channel, string requestId)
    {
        await _db.PublishAsync(channel, requestId);
    }

}
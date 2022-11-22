using System.Text.Json;
using StackExchange.Redis;

namespace Chat.AppCore.Services.CacheService;

public class CacheService : ICacheService
{
    private IDatabase _db;
    
    public CacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _db = connectionMultiplexer.GetDatabase();
    }
    
    public async Task SetRecordAsync<T>(string key, T data, TimeSpan? expireTime = null)
    {
        var jsonData =  JsonSerializer.Serialize(data);
        await _db.StringSetAsync(
                key:key, 
                value:jsonData, 
                expiry: expireTime?? TimeSpan.FromSeconds(120))
            .ConfigureAwait(false);
    }

    public async Task<T?> GetRecordAsync<T>(string key)
    {
        var jsonData = await _db.StringGetAsync(key).ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(jsonData!);
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

}
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Chat.AppCore.Extensions;

public static class DistributedCacheExtentions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string recordId, 
        T data,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpiredTime = null)
    {
        var options = new DistributedCacheEntryOptions();
        
        // определяем сколько запись в кеше будет жить 
        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(120);
        
        // определяем время, через которое удаляется запись из кеша, если не используется
        options.SlidingExpiration = unusedExpiredTime;
        
        var jsonData =  JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }

    public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);
        if (jsonData is null) return default(T);
        return JsonSerializer.Deserialize<T>(jsonData);
    }
}
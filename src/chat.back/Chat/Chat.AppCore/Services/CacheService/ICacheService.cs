namespace Chat.AppCore.Services.CacheService;

public interface ICacheService
{
    public Task SetRecordAsync<T>(string key, T data, TimeSpan? expireTime = null);
    public Task<T?> GetRecordAsync<T>(string key);
    public Task IncrementAsync(string key);
    public Task AppendRecordAsync<T>(string key, T data);
}
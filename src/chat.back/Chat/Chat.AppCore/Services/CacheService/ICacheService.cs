namespace Chat.AppCore.Services.CacheService;

public interface ICacheService
{
    public Task<string?> Get(string key);
    public Task Set(string key, string value);

}
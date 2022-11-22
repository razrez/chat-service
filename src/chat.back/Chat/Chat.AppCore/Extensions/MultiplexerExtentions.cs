using Chat.AppCore.Services.CacheService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Chat.AppCore.Extensions;

public static class MultiplexerExtentions
{
    public static IServiceCollection AddMultiplexer(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddSingleton<IConnectionMultiplexer>(x => 
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")));
        collection.AddSingleton<ICacheService, CacheService>();
        
        return collection;
    }
}
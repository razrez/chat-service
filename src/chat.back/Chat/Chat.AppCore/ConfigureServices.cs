using Chat.AppCore.Common.Models;
using Chat.AppCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.AppCore;

public static class ConfigureServices
{
    public static IServiceCollection AddAppCore(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        //Adds Redis distributed caching service
        serviceCollection.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration.GetConnectionString("RedisConnection");
            opt.InstanceName = "RedisChat_";
        });
        return serviceCollection;
    }
}
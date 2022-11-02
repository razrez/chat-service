using Chat.AppCore.Common.Models;
using Chat.AppCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.AppCore;

public static class ConfigureServices
{
    public static IServiceCollection AddAppCore(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        //var mongpOpt = configuration.GetSection("Mongodb");
        serviceCollection.AddAwsService(configuration);
        return serviceCollection;
    }
}
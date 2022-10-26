using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.AppCore.Extensions;

public static class AddAWS
{
    public static IServiceCollection AddAWSService<T>(this IServiceCollection collection, AWSOptions options, ServiceLifetime lifetime = ServiceLifetime.Singleton) where T : IAmazonService
    {
        return collection;
    }
}
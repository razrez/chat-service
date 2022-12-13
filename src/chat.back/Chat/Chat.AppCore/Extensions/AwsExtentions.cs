using Amazon;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.AppCore.Extensions;

public static class AwsExtentions
{
    public static IServiceCollection AddAwsService(this IServiceCollection collection, IConfiguration configuration)
    {
        var s3Config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USWest1,
            ForcePathStyle = true,
            ServiceURL = configuration["AWS:ServiceURL"]
        };
        collection.AddSingleton<IAmazonS3>(_ => GetS3Client(configuration, s3Config));
        
        return collection;
    }
    private static AmazonS3Client GetS3Client(IConfiguration configuration, AmazonS3Config s3Config) 
        => new(configuration["AWS:AccessKey"], configuration["AWS:AccessSecret"], s3Config);
}
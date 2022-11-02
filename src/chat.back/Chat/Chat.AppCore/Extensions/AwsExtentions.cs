using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.AppCore.Extensions;

public static class AwsExtentions
{
    public static IServiceCollection AddAwsService(this IServiceCollection collection, IConfiguration configuration)
    {
        var awsConfig = configuration.GetAWSOptions();
        var awsOpt = new AWSOptions
        {
            Credentials = new BasicAWSCredentials(
                configuration["AWS:AccessKey"],
                configuration["AWS:AccessSecret"]),

            DefaultClientConfig =
            {
                ServiceURL = configuration["AWS:ServiceUrl"],
            }
        };

        collection.AddDefaultAWSOptions(awsOpt);
        collection.AddAWSService<IAmazonS3>();
        
        return collection;
    }
}
using Chat.AppCore;
using Chat.AppCore.Common.Models;
using Chat.AppCore.Extensions;
using Chat.AppCore.Services;
using Chat.BackgroundService.Consumers;
using Chat.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(
        (_, services) =>
    {
        services.AddInfrastructure(config);
        //MongoDB Metadata Service
        services.AddAwsService(config);
        services.Configure<MetadataDbSettings>(config.GetSection("MongoDB"));
        services.AddMultiplexer(config);
        services.AddSingleton<MetadataService>();
        services.AddHostedService<MessageConsumer>();
        services.AddHostedService<FileConsumer>();
        services.AddHostedService<MetadataConsumer>();
    })
    .Build();

await host.RunAsync();
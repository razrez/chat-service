﻿using Chat.AppCore.Common.Models;
using Chat.AppCore.Extensions;
using Chat.AppCore.Services;
using Chat.BackgroundService.Consumers;
using Chat.Infrastructure;

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
        services.Configure<MongoDbSettings>(config.GetSection("MongoDB"));
        services.AddMultiplexer(config);
        services.AddSingleton<MetadataService>();
        services.AddHostedService<MessageConsumer>();
        services.AddHostedService<FileConsumer>();
        services.AddHostedService<MetadataConsumer>();
        services.AddHostedService<StatisticConsumer>();
    })
    .Build();

await host.RunAsync();
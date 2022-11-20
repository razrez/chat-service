using System.Text.Json;
using Chat.AppCore.Common.DTO;
using Chat.AppCore.Services;
using Chat.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.BackgroundService.Consumers;

// saves metadata and move file to persistent bucket
public class MetadataConsumer : Microsoft.Extensions.Hosting.BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private ConnectionFactory _connectionFactory;
    private const string QueueName = "metadata-queue";
    private readonly ILogger<MessageConsumer> _logger;
    private readonly MetadataService _metadata;
    private readonly IDistributedCache _cache;

    public MetadataConsumer(ILogger<MessageConsumer> logger, MetadataService metadata, IDistributedCache cache)
    {
        _logger = logger;
        _metadata = metadata;
        _cache = cache;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        
        _channel.ExchangeDeclare(exchange:"logs", type: ExchangeType.Fanout );
        _channel.QueueDeclare(queue: "metadata-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _logger.LogInformation($"[{QueueName}] has started.");

        return base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _connection.Close();
        _logger.LogInformation("MetadataConsumer is stopped");
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var metadataFile = JsonSerializer.Deserialize<MetadataFile>(body);
                
                if (metadataFile != null)
                {
                    await _metadata.CreateAsync(metadataFile);
                }
            }
            catch (Exception exception)
            {
                _logger.LogWarning("{name}Exception: " + exception.Message, "ARG0");
            }
        };

        _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

        await Task.CompletedTask;
    }
}
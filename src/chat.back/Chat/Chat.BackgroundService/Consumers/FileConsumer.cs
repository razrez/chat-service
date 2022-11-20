using System.Text.Json;
using Amazon.S3;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CopyObjectRequest = Amazon.S3.Model.CopyObjectRequest;
using PutObjectRequest = Amazon.S3.Model.PutObjectRequest;

namespace Chat.BackgroundService.Consumers;

public class FileConsumer : Microsoft.Extensions.Hosting.BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private ConnectionFactory _connectionFactory;
    private const string QueueName = "file-queue";
    private readonly ILogger<MessageConsumer> _logger;
    private readonly IDistributedCache _cache;
    private readonly IAmazonS3 _s3Client;

    public FileConsumer(ILogger<MessageConsumer> logger, IDistributedCache cache, IAmazonS3 s3Client)
    {
        _logger = logger;
        _cache = cache;
        _s3Client = s3Client;
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
        _channel.QueueDeclare(queue: "file-queue",
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
        _logger.LogInformation("FileConsumer is stopped");
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                // создаем корзину, если ее нет
                var bucketExists = await _s3Client.DoesS3BucketExistAsync("persistent");
                if (!bucketExists) await _s3Client.PutBucketAsync("persistent");
                
                var body = ea.Body.ToArray();
                var copyObjectRequest = JsonSerializer.Deserialize<CopyObjectRequest>(body);

                // файл двигается из temp в persistent bucket 
                
                //var file = s3Object.ResponseStream;
                var s3Object = await _s3Client.GetObjectAsync(
                    copyObjectRequest!.SourceBucket, 
                    copyObjectRequest.SourceKey,
                    cancellationToken);
                
                copyObjectRequest.ETagToMatch = s3Object.ETag;
                await _s3Client.CopyObjectAsync(copyObjectRequest, cancellationToken);
                
                // далее идёт инкрементация в кеш
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
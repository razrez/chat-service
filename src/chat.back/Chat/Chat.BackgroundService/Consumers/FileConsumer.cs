using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Chat.AppCore.Common.DTO;
using Chat.AppCore.Services.CacheService;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CopyObjectRequest = Amazon.S3.Model.CopyObjectRequest;

namespace Chat.BackgroundService.Consumers;

public class FileConsumer : Microsoft.Extensions.Hosting.BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private ConnectionFactory _connectionFactory;
    private const string QueueName = "file-queue";
    private readonly ILogger<MessageConsumer> _logger;
    private readonly ICacheService _cache;
    private readonly IAmazonS3 _s3Client;

    public FileConsumer(ILogger<MessageConsumer> logger, ICacheService cache, IAmazonS3 s3Client)
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
                var copyRequest = JsonSerializer.Deserialize<CopyRequest>(body);

                // файл двигается из temp в persistent bucket 
                var s3Object = await _s3Client.GetObjectAsync(
                    copyRequest!.SourceBucket, 
                    copyRequest.SourceKey,
                    cancellationToken);
                
                var copyObjectRequest = new CopyObjectRequest
                {
                    SourceBucket = copyRequest.SourceBucket,
                    SourceKey = copyRequest.SourceKey,
                    DestinationBucket = copyRequest.DestinationBucket,
                    DestinationKey = copyRequest.DestinationKey,
                    ETagToMatch = s3Object.ETag,
                    CannedACL = S3CannedACL.PublicRead,
                };
                
                await _s3Client.CopyObjectAsync(copyObjectRequest, cancellationToken);

                // далее идёт инкрементация в кеш
                if (copyRequest.RequestId != null)
                {
                    await _cache.IncrementAsync(copyRequest.RequestId);
                    // публикуем запрос для проверки синхронизации - RedisSubscriber в Chat.API обрабатывает 
                    await _cache.SyncRequest("sync", copyRequest.RequestId);
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
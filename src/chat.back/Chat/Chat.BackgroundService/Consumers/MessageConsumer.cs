using System.Text.Json;
using Chat.AppCore.Common.DTO;
using Chat.Infrastructure.Persistence.Repository;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.BackgroundService.Consumers;

public class MessageConsumer : Microsoft.Extensions.Hosting.BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private ConnectionFactory _connectionFactory;
    private const string QueueName = "message-queue";
    private readonly ILogger<MessageConsumer> _logger;
    private readonly IChatRepository _context;

    public MessageConsumer(IChatRepository context, ILogger<MessageConsumer> logger)
    {
        _context = context;
        _logger = logger;
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
        _channel.QueueDeclare(queue: "message-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _logger.LogInformation($"[{QueueName}] has started.");

        return base.StartAsync(cancellationToken);
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<SaveMessageDto>(body);
                await _context.SaveMessage(message.Room, message.User, message.Message);
            }
            catch (Exception exception)
            {
                _logger.LogWarning("{name}Exception: " + exception.Message, "ARG0");
            }
        };

        _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

        await Task.CompletedTask;
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        _connection.Close();
        _logger.LogInformation("Consumer is stopped");
    }

}
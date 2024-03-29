using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Chat.AppCore.Publisher;

/// <summary>
/// я знаю, что код продуплировался, потом хочу поиграться с "channel.QueueDeclare"
/// </summary>
public class MessagePublisher : IMessagePublisher
{
    public void SaveMessage<T>(T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq"
        };
        
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.ExchangeDeclare("logs", ExchangeType.Fanout);
        channel.QueueDeclare(queue: "message-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: "",
            routingKey: "message-queue",
            basicProperties: null,
            body: body);
    }


    public void UploadFileOrMeta<T>(T data, string queueName)
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq"
        };
        
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.ExchangeDeclare("logs", ExchangeType.Fanout);
        channel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var jsonMessage = JsonSerializer.Serialize(data);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body);
    }

    public void UpdateStatistic<T>(T? data, string queueName)
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq"
        };
        
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.ExchangeDeclare("logs", ExchangeType.Fanout);
        channel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var jsonMessage = JsonSerializer.Serialize(data);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body);
    }
}
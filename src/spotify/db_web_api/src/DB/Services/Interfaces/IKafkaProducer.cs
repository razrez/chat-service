namespace DB.Services.Interfaces;

public interface IKafkaProducer
{
    Task<bool> SendMessage(string topic, string message);
}
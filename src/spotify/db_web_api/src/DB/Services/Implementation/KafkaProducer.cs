using Confluent.Kafka;
using DB.Services.Interfaces;

namespace DB.Services.Implementation;

public class KafkaProducer : IKafkaProducer
{
    private static readonly ProducerConfig ProducerConfig = new()
    {
        BootstrapServers = "localhost:9092",
    };
    
    public async Task<bool> SendMessage(string topic, string message)
    {
        using var producer = new ProducerBuilder<Null, string>(ProducerConfig).Build();
        try
        {
            await producer.ProduceAsync(
                topic, new Message<Null, string>
                {
                    Value = message
                });
            
            return true;
        }
        catch (ProduceException<string, string> e)
        {
            Console.WriteLine($"Kafka.StatisticConsumer.Error: Failed to deliver message: {e.Message} [{e.Error.Code}");
            return false;
        }
    }
}
using Confluent.Kafka;

namespace Chat.BackgroundService.Consumers;

public class StatisticConsumer : Microsoft.Extensions.Hosting.BackgroundService
{
    private static readonly ConsumerConfig ConsumerConfig = new()
    {
        BootstrapServers = "localhost:9092",
    };
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(ConsumerConfig).Build();
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                consumer.Subscribe("spotify.statistics.increment");
                var cr = consumer.Consume(stoppingToken);
                
                Console.WriteLine(cr.Message.Value); //todo: логика отображения
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Kafka.StatisticConsumer.Error: {e.Error.Reason}");
            }
        }
    }
}
using Chat.AppCore.Publisher;
using Chat.AppCore.Services;
using Chat.Domain.Entities;
using Confluent.Kafka;

namespace Chat.BackgroundService.Consumers;

public class StatisticConsumer : Microsoft.Extensions.Hosting.BackgroundService
{
    private static readonly ConsumerConfig ConsumerConfig = new()
    {
        GroupId = "1",
        BootstrapServers = "kafka:9092",
    };

    private readonly StatisticService _statisticService;
    private readonly IMessagePublisher _publisher;

    public StatisticConsumer(StatisticService statisticService, IMessagePublisher publisher)
    {
        _statisticService = statisticService;
        _publisher = publisher;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(ConsumerConfig).Build();
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                consumer.Subscribe("spotify.statistics.increment");
                var cr = consumer.Consume(stoppingToken);
                
                // increment listens of song in Mongo
                var songId = cr.Message.Value;
                Console.WriteLine(songId); //todo: логика отображения
                await _statisticService.IncrementAsync(songId);
                
                // then publish message for mobile client who catches changed song's stat
                var updatedSongStat = await _statisticService.GetAsync(songId);
                Console.WriteLine(updatedSongStat.Listens);
                _publisher.UpdateStatistic(new
                {
                    songId = updatedSongStat.SongId,
                    listens = updatedSongStat.Listens
                }, "stats-queue");
                
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Kafka.StatisticConsumer.Error: {e.Error.Reason}");
            }
        }
    }
}
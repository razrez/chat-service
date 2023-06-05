using DB.Services.Interfaces;

namespace DB.Services.Implementation;

public class StatisticService : IStatisticService
{
    public StatisticService(IKafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    private IKafkaProducer _kafkaProducer { get; set; }

    public async Task<List<object>> GetAll() => new List<object>();

    public async Task<bool> Add(int songId) =>
        await _kafkaProducer.SendMessage("spotify.statistics.increment", songId.ToString());

}
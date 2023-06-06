using Chat.AppCore.Common.Models;
using Chat.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chat.AppCore.Services;

public class StatisticService
{
    private readonly IMongoCollection<Statistic> _statisticCollection;

    public StatisticService(IOptions<MongoDbSettings> metadataDbSettings)
    {
        var mongoClient = new MongoClient(
            metadataDbSettings.Value.ConnectionString);
        
        var mongoDb = mongoClient.GetDatabase(
            metadataDbSettings.Value.DatabaseName);
        
        _statisticCollection = mongoDb.GetCollection<Statistic>(
            metadataDbSettings.Value.MetadataCollectionName);
    }

    public async Task<List<Statistic>> GetAll() =>
        await _statisticCollection.Find(_ => true).ToListAsync();

    public async Task<Statistic> GetAsync(string id) =>
        await _statisticCollection.Find(x => x.SongId == id).FirstOrDefaultAsync();

    public async Task IncrementAsync(string songId)
    {
        var record = await _statisticCollection
            .Find(x => x.SongId == songId)
            .FirstOrDefaultAsync();
        
        if (record != null)
        {
            var update = new BsonDocument("$set", new BsonDocument("Listens", record.Listens + 1));
            var resUpdate = await _statisticCollection.UpdateOneAsync(x => x.SongId == songId, update);
            Console.WriteLine(resUpdate.ToString());
        }

        else
        {
            var newStats = new Statistic()
            {
                SongId = songId,
                Listens = 1
            };

            await _statisticCollection.InsertOneAsync(newStats);
            Console.WriteLine(newStats.ToString());
        }

    }

}
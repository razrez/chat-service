using Chat.AppCore.Common.Models;
using Chat.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Chat.AppCore.Services;

public class MetadataService
{
    private readonly IMongoCollection<MetadataFile> _metaCollection;

    public MetadataService(IOptions<MongoDbSettings> metadataDbSettings)
    {
        var mongoClient = new MongoClient(
            metadataDbSettings.Value.ConnectionString);
        
        var mongoDb = mongoClient.GetDatabase(
            metadataDbSettings.Value.DatabaseName);
        
        _metaCollection = mongoDb.GetCollection<MetadataFile>(
            metadataDbSettings.Value.MetadataCollectionName);
    }

    public async Task<List<MetadataFile>> GetAllSync() =>
        await _metaCollection.Find(_ => true).ToListAsync();
    
    public async Task<List<MetadataFile>> GetAsyncByRoom(string room) =>
        await _metaCollection.Find(r => r.RoomName == room).ToListAsync();
    
    public async Task<MetadataFile> GetAsync(string id) =>
        await _metaCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(MetadataFile metadataFile) =>
        await _metaCollection.InsertOneAsync(metadataFile);
    public void Create(MetadataFile metadataFile) =>
        _metaCollection.InsertOne(metadataFile);

}
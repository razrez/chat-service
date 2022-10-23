using Chat.AppCore.Common.Interfaces;
using Chat.AppCore.Common.Models;
using Chat.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Chat.Infrastructure.Persistence.Services;

public class MetadataService : IMetadataContext
{
    public IMongoCollection<MetadataFile> MetaFiles { get; set; }
    private readonly IMongoCollection<MetadataFile> _metaCollection;

    public MetadataService(IOptions<MetadataDbSettings> metadataDbSettings)
    {
        var mongoClient = new MongoClient(
            metadataDbSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(
            metadataDbSettings.Value.DatabaseName);
        
        _metaCollection = mongoDb.GetCollection<MetadataFile>(
            metadataDbSettings.Value.MetadataCollectionName);

        MetaFiles = _metaCollection;
    }

    public async Task<IAsyncCursor<MetadataFile>> GetAsync(string id) =>
        await _metaCollection.FindAsync(x => x.FileName == id);

}
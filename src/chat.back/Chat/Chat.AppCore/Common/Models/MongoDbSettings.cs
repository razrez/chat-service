namespace Chat.AppCore.Common.Models;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string MetadataCollectionName { get; set; } = null!;
}
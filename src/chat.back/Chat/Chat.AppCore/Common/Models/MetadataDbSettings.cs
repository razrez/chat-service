namespace Chat.AppCore.Common.Models;

public class MetadataDbSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string MetadataCollectionName { get; set; } = null!;
}
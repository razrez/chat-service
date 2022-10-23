using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Domain.Entities;

public class MetadataFile
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string FileName { get; set; } = null!;

    [BsonElement("Content-Type")]
    public string? ContentType { get; set; }
}
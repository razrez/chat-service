using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Domain.Entities;

public class MetadataFile
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("Name")]
    public string FileName { get; set; } = null!;
    
    [BsonElement("Content-Type")]
    public string? ContentType { get; set; }
    
    [BsonElement("Room")]
    public string RoomName { get; set; } = null!;
    
    [BsonElement("User")]
    public string User { get; set; } = null!;
}
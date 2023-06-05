using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Domain.Entities;

public class Statistic
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? SongId { get; set; }
    
    [BsonElement("Listens")]
    public int Listens { get; set; }
}
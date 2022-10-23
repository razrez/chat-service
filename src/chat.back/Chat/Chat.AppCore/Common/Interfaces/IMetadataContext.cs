using Chat.Domain.Entities;
using MongoDB.Driver;

namespace Chat.AppCore.Common.Interfaces;

public interface IMetadataContext
{
    IMongoCollection<MetadataFile> MetaFiles { get; set; }
    
}
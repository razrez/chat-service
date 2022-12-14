using Chat.Domain.Entities;

namespace Chat.AppCore.Common.DTO;

public class CacheObject
{
    public MetadataFile? Metadata { get; set; }
    public string? FileId { get; set; }
}
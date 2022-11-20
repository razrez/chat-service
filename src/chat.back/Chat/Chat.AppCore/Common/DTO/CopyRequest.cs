using Amazon.S3.Model;

namespace Chat.AppCore.Common.DTO;

public class CopyRequest : CopyObjectRequest
{
    public string? RequestId { get; set; }
}
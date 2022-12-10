using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using Chat.API.Publisher;
using Chat.AppCore.Common.DTO;
using Chat.AppCore.Services.CacheService;
using PutObjectRequest = Amazon.S3.Model.PutObjectRequest;

namespace Chat.API.Controllers;

[Route("api/files")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IAmazonS3 _s3Client;
    private readonly IMessagePublisher _publisher;
    private readonly ICacheService _cache;
    public FileController(IAmazonS3 s3Client, IMessagePublisher publisher, ICacheService cache)
    {
        _s3Client = s3Client;
        _publisher = publisher;
        _cache = cache;
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file, string roomName, string prefix, string reqId)
    {
        // prefix - userName
        // теперь 2 корзины: temp and persistant 
        
        // создаем корзину, если ее нет
        var bucketExists = await _s3Client.DoesS3BucketExistAsync("temp");
        if (!bucketExists) await _s3Client.PutBucketAsync("temp");
        
        var request = new PutObjectRequest
        {
            BucketName = "temp",
            Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{roomName}/{prefix.TrimEnd('/')}/{file.FileName}",
            InputStream = file.OpenReadStream(),
            CannedACL = S3CannedACL.PublicRead
        };

        request.Metadata.Add("FileName", file.FileName);
        request.Metadata.Add("ContentType", file.Headers.ContentType);
        request.Metadata.Add("RoomName", roomName);
        request.Metadata.Add("User", prefix);
        
        // сохраняяем в Temp Bucket
        await _s3Client.PutObjectAsync(request);
        
        // этот id передаётся с фронта
        string recordKey = $"FileId_{reqId}";
        await _cache.SetRecordAsync(recordKey, request.Key); // типо File ID, который потом связывается с метой
        
        // отправляем запрос в очередь, который потом уже вызовется в обработчике события на Consumer'е 
        var copyRequest = new CopyRequest()
        {
            RequestId = reqId,
            SourceBucket = "temp",
            SourceKey = request.Key,
            DestinationBucket = "persistent",
            DestinationKey = request.Key,
            CannedACL = S3CannedACL.PublicRead,
        };
        
        _publisher.UploadFileOrMeta(copyRequest, "file-queue");
        
        return Ok(request.Key);
    }
    
    [HttpGet("get-by-key")]
    public async Task<IActionResult> GetFileByKeyAsync(string key)
    {
        var bucketExists = await _s3Client.DoesS3BucketExistAsync("persistent");
        if (!bucketExists) return NotFound($"Bucket persistent does not exist.");
        var s3Object = await _s3Client.GetObjectAsync("persistent", key);
        
        return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteFileAsync(string bucketName, string key)
    {
        var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
        if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist");
        await _s3Client.DeleteObjectAsync(bucketName, key);
        return NoContent();
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllFilesAsync(string bucketName, string? prefix)
    {
        var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
        if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
        var request = new ListObjectsV2Request()
        {
            BucketName = bucketName,
            Prefix = prefix
        };
        var result = await _s3Client.ListObjectsV2Async(request);
        var s3Objects = result.S3Objects.Select(s =>
        {
            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = s.Key,
                Expires = DateTime.UtcNow.AddMinutes(1)
            };
            return new S3ObjectDto()
            {
                Name = s.Key.ToString(),
                PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
            };
        });
        return Ok(s3Objects);
    }
}
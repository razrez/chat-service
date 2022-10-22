using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using Chat.AppCore.Common.DTO;
using PutObjectRequest = Amazon.S3.Model.PutObjectRequest;

namespace Chat.API.Controllers;

[Route("api/files")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IAmazonS3 _s3Client;
    public FileController(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file, string bucketName, string? prefix)
    {
        var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
        if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix.TrimEnd('/')}/{file.FileName}",
            InputStream = file.OpenReadStream(),
            CannedACL = S3CannedACL.PublicRead
        };
        
        request.Metadata.Add("Content-Type", file.ContentType);
        await _s3Client.PutObjectAsync(request);
        return Ok($"File {prefix}/{file.FileName} uploaded to S3 successfully!");
    }
    
    [HttpGet("get-by-key")]
    public async Task<IActionResult> GetFileByKeyAsync(string bucketName, string key)
    {
        var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
        if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
        var s3Object = await _s3Client.GetObjectAsync(bucketName, key);
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
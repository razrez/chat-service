using Amazon.S3;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[Route("api/bucket")]
[ApiController]
public class BucketController : ControllerBase
{
    private readonly IAmazonS3 _s3Client;
    public BucketController(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateBucketAsync(string bucketName)
    {
        var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
        if (bucketExists) return BadRequest($"Bucket {bucketName} already exists.");
        await _s3Client.PutBucketAsync(bucketName);
        return Ok($"Bucket {bucketName} created.");
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteBucketAsync(string bucketName)
    {
        await _s3Client.DeleteBucketAsync(bucketName);
        return NoContent();
    }
}
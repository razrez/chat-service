using Amazon.S3;
using Amazon.S3.Endpoints;
using Amazon.S3.Internal;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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
        /*var putBucketRequest = new PutBucketRequest
        {
            BucketName = bucketName,
            UseClientRegion = true
        };
        var response = await _s3Client.PutBucketAsync(putBucketRequest);
        return Ok(_s3Client.Config.ServiceURL);*/
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteBucketAsync(string bucketName)
    {
        await _s3Client.DeleteBucketAsync(bucketName);
        return NoContent();
    }
}
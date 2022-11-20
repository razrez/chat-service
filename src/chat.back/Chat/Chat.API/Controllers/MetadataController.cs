using Chat.API.Publisher;
using Chat.AppCore.Common.DTO;
using Chat.AppCore.Extensions;
using Chat.AppCore.Services;
using Chat.AppCore.Services.CacheService;
using Chat.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Chat.API.Controllers;

[ApiController]
[Route("api/file-metadata")]
public class MetadataController : ControllerBase
{
    private readonly MetadataService _metadata;
    private readonly IDistributedCache _cache;
    private readonly IMessagePublisher _publisher;
    

    public MetadataController(MetadataService metadata, IDistributedCache cache, IMessagePublisher publisher)
    {
        _metadata = metadata;
        _cache = cache;
        _publisher = publisher;
    }

    [HttpGet("get-by-id")]
    public async Task<ActionResult<MetadataFile>> Get(string id)
    {
        var metadataFile = await _cache.GetRecordAsync<MetadataFile>(id);
        
        if (metadataFile is null)
        {
            metadataFile = await _metadata.GetAsync(id);
            await _cache.SetRecordAsync(id, metadataFile);
        }

        return Ok(metadataFile);
    }

    [HttpGet("get-by-room")]
    public async Task<List<MetadataFile>> GetByRoom(string room)
    { 
        string recordKey = $"Metadata_{room}";
        var metaFiles = await _cache.GetRecordAsync<List<MetadataFile>>(room);
        
        // если в кеше ничего
        if (metaFiles is null)
        {
            metaFiles = await _metadata.GetAsyncByRoom(room);
            
            //закием в кеш
            await _cache.SetRecordAsync(recordKey, metaFiles);
            
            return metaFiles;
        }
        
        return metaFiles;
    }

    [HttpPost]
    public async Task<IActionResult> Create(MetadataDto metadataDto)
    {
        var newMeta = new MetadataFile
        {
            FileName = metadataDto.FileName,
            ContentType = metadataDto.ContentType,
            RoomName = metadataDto.RoomName,
            User = metadataDto.User,
            RequestId = metadataDto.RequestId
        };
        
        // этот id передаётся с фронта
        string recordKey = $"RequestId";
        
        await _cache.SetRecordAsync(recordKey, newMeta); // caching 
        
        // отправка в очередь для сохранения в монгу
        _publisher.UploadFileOrMeta(metadataDto, "metadata-queue");

        return Ok(newMeta.Id);
        /*return CreatedAtAction(
            nameof(Get),
            new { id = newMeta.Id },
            newMeta);*/
    }
}

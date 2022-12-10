using Chat.API.Publisher;
using Chat.AppCore.Common.DTO;
using Chat.AppCore.Services;
using Chat.AppCore.Services.CacheService;
using Chat.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("api/file-metadata")]
public class MetadataController : ControllerBase
{
    private readonly MetadataService _metadata;
    private readonly ICacheService _cache;
    private readonly IMessagePublisher _publisher;
    
    public MetadataController(MetadataService metadata, ICacheService cache, IMessagePublisher publisher)
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
        // этот id передаётся с фронта
        var recordKey = $"Metadata_{metadataDto.RequestId}";
        await _cache.SetRecordAsync(recordKey, metadataDto);
        
        // отправка в очередь для сохранения в монгу
        _publisher.UploadFileOrMeta(metadataDto, "metadata-queue");

        return Ok(metadataDto);
        /*return CreatedAtAction(
            nameof(Get),
            new { id = newMeta.Id },
            newMeta);*/
    }
}

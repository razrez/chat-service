using Chat.AppCore.Common.DTO;
using Chat.Domain.Entities;
using Chat.Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("api/file-metadata")]
public class MetadataController : ControllerBase
{
    //тут метаданные будут доставаться из монго

    private readonly MetadataService _metadata;

    public MetadataController(MetadataService metadata) =>
        _metadata = metadata;
    
    [HttpGet("get-by-id")]
    public async Task<ActionResult<MetadataFile>> Get(string id)
    {
        var metadataFile = await _metadata.GetAsync(id);

        if (metadataFile is null)
        {
            return NotFound();
        }

        return metadataFile;
    }

    [HttpGet("get-by-room")]
    public async Task<List<MetadataFile>> GetByRoom(string room) =>
        await _metadata.GetAsyncByRoom(room);

    [HttpPost]
    public async Task<IActionResult> Create(MetadataDto metadataDto)
    {
        var newMeta = new MetadataFile
        {
            FileName = metadataDto.FileName,
            ContentType = metadataDto.ContentType,
            RoomName = metadataDto.RoomName,
            User = metadataDto.User
        };
        
        await _metadata.CreateAsync(newMeta);
        return CreatedAtAction(
            nameof(Get),
            new { id = newMeta.Id },
            newMeta);
    }
}
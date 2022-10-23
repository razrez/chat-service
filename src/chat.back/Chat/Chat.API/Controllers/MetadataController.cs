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

    [HttpGet]
    public async Task<List<MetadataFile>> Get() =>
        await _metadata.GetAsync();

    [HttpGet]
    public async Task<IActionResult> Get(string id)
    {
        var metaFile = await _metadata.GetAsync(id);
        
        return metaFile is null ? NotFound() : Ok(metaFile);
    }
}
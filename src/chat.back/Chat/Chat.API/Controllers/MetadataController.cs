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

    [HttpGet("get-all")]
    public async Task<List<MetadataFile>> GetRoom(string room) =>
        await _metadata.GetAsyncByRoom(room);
}
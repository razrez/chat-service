using Chat.AppCore.Common.DTO;
using Chat.AppCore.Services.CacheService;
using Chat.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RedisController : Controller
{

    private readonly ICacheService _cacheService;

    public RedisController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }
    
    [HttpPost("set")]
    public async Task<IActionResult> SetСacheValue([FromBody] MetadataFile model)
    {
        await _cacheService.Set(model.Id, model.FileName);
        return Ok();
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetCacheValue([FromQuery] string key)
    {
        return Ok(await _cacheService.Get(key));
    }
    

    
}

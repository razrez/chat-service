using DB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Confluent.Kafka;

[ApiController]
[Route("api/statistic")]
[Produces("application/json")]
public class StatisticController : ControllerBase
{
    public StatisticController(IStatisticService statisticService)
    {
        _statisticService = statisticService;
    }

    protected IStatisticService _statisticService { get; set; }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _statisticService.GetAll();
        return Ok(result);
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> Add(int songId)
    {
        var result = await _statisticService.Add(songId);
        return result ? Ok() : BadRequest("иди отдыхай, ботик)");
    }
}
using Chat.AppCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("api/statistic")]
[Produces("application/json")]
public class StatisticController : ControllerBase
{
    private readonly StatisticService _statisticService;

    public StatisticController(StatisticService statisticService)
    {
        _statisticService = statisticService;
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _statisticService.GetAll();
        return Ok(result);
    }
}
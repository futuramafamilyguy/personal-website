using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Infrastructure.Data.Visits;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("stats")]
public class StatisticsController : ControllerBase
{
    private readonly VisitStatisticsRepository _visitStatisticsRepository;

    public StatisticsController(VisitStatisticsRepository visitStatisticsRepository)
    {
        _visitStatisticsRepository = visitStatisticsRepository;
    }

    [HttpGet("")]
    public async Task<ActionResult> GetCinemasAsync()
    {
        var stats = await _visitStatisticsRepository.GetVisitStatisticsAsync();

        return Ok(stats);
    }
}

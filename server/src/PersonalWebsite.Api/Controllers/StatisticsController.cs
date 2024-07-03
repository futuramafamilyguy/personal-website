using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PersonalWebsite.Infrastructure.Data.Visits;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("stats")]
public class StatisticsController : ControllerBase
{
    private readonly VisitStatisticsRepository _visitStatisticsRepository;
    private readonly VisitExclusionConfiguration _configuration;

    public StatisticsController(
        VisitStatisticsRepository visitStatisticsRepository,
        IOptions<VisitExclusionConfiguration> configuration
    )
    {
        _visitStatisticsRepository = visitStatisticsRepository;
        _configuration = configuration.Value;
    }

    [HttpGet("")]
    public async Task<ActionResult> GetCinemasAsync()
    {
        var stats = await _visitStatisticsRepository.GetVisitStatisticsAsync();

        return Ok(stats);
    }

    [Authorize(Policy = "AuthenticatedPolicy")]
    [HttpGet("disable-visit")]
    public ActionResult SetVisitExclusionCookie()
    {
        HttpContext.Response.Cookies.Append(
            "ExcludeVisit",
            _configuration.ExcludeVisitCookieValue,
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) }
        );

        return Ok("Visit exclusion cookie set.");
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PersonalWebsite.Api.VisitTracking;
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
    public async Task<IActionResult> GetCinemasAsync()
    {
        var stats = await _visitStatisticsRepository.GetVisitStatisticsAsync();

        return Ok(stats);
    }

    [HttpPost("increment")]
    public IActionResult IncrementVisitCountAsync() => NoContent();

    [Authorize(Policy = "DisableVisitPolicy")]
    [HttpPost("disable-tracking")]
    public IActionResult SetVisitExclusionCookie()
    {
        HttpContext.Response.Cookies.Append(
            "ExcludeVisit",
            _configuration.ExcludeVisitCookieValue,
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.None,
                Secure = true
            }
        );

        return Ok("Visit exclusion cookie set.");
    }
}

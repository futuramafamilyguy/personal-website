using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CinemasController : ControllerBase
{
    private readonly ICinemaService _cinemaService;
    private readonly IPictureCinemaOrchestrator _pictureCinemaOrchestrator;
    private readonly ILogger<CinemasController> _logger;

    public CinemasController(
        ICinemaService cinemaService,
        IPictureCinemaOrchestrator pictureCinemaOrchestrator,
        ILogger<CinemasController> logger)
    {
        _cinemaService = cinemaService;
        _pictureCinemaOrchestrator = pictureCinemaOrchestrator;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<ActionResult> GetCinemasAsync()
    {
        var cinemas = await _cinemaService.GetCinemasAsync();

        return Ok(cinemas);
    }

    [Authorize(Policy = "AuthenticatedPolicy")]
    [HttpPost("")]
    public async Task<ActionResult> CreateCinemaAsync([FromBody]CreateCinemaRequest request)
    {
        if (request is null) return BadRequest("Cinema data is null");

        var cinema = await _cinemaService.AddCinemaAsync(request.Name, request.City);

        return Ok(cinema);
    }

    [Authorize(Policy = "AuthenticatedPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCinemaAsync(string id, [FromBody]CreateCinemaRequest request)
    {
        if (request is null) return BadRequest("Cinema data is null");

        var updatedCinema = await _pictureCinemaOrchestrator.UpdateCinemaAndAssociatedPicturesAsync(id, request.Name, request.City);

        return Ok(updatedCinema);
    }

    [Authorize(Policy = "AuthenticatedPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCinemaAsync(string id)
    {
        try
        {
            await _pictureCinemaOrchestrator.ValidateAndDeleteCinema(id);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"Failed to delete cinema {id}");
            return Conflict($"Failed to delete cinema {id}");
        }
    }
}

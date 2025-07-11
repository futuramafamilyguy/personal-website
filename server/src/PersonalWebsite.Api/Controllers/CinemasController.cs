using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CinemasController : ControllerBase
{
    private readonly ICinemaService _cinemaService;
    private readonly IMovieCinemaOrchestrator _movieCinemaOrchestrator;

    public CinemasController(
        ICinemaService cinemaService,
        IMovieCinemaOrchestrator movieCinemaOrchestrator
    )
    {
        _cinemaService = cinemaService;
        _movieCinemaOrchestrator = movieCinemaOrchestrator;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetCinemasAsync()
    {
        var cinemas = await _cinemaService.GetCinemasAsync();

        return Ok(cinemas);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("")]
    public async Task<IActionResult> CreateCinemaAsync([FromBody] CreateCinemaRequest request)
    {
        var cinema = await _cinemaService.AddCinemaAsync(request.Name, request.City);

        return Ok(cinema);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCinemaAsync(
        string id,
        [FromBody] CreateCinemaRequest request
    )
    {
        var updatedCinema = await _movieCinemaOrchestrator.UpdateCinemaAndAssociatedMoviesAsync(
            id,
            request.Name,
            request.City
        );

        return Ok(updatedCinema);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCinemaAsync(string id)
    {
        try
        {
            await _movieCinemaOrchestrator.ValidateAndDeleteCinema(id);

            return NoContent();
        }
        catch (DomainValidationException ex)
        {
            return Conflict(ex);
        }
    }
}

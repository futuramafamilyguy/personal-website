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
    private readonly IPictureCinemaOrchestrator _pictureCinemaOrchestrator;

    public CinemasController(
        ICinemaService cinemaService,
        IPictureCinemaOrchestrator pictureCinemaOrchestrator
    )
    {
        _cinemaService = cinemaService;
        _pictureCinemaOrchestrator = pictureCinemaOrchestrator;
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
        var updatedCinema = await _pictureCinemaOrchestrator.UpdateCinemaAndAssociatedPicturesAsync(
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
            await _pictureCinemaOrchestrator.ValidateAndDeleteCinema(id);

            return NoContent();
        } catch (CinemaHasAssociatedPicturesException ex)
        {
            return Conflict(ex);
        }
    }
}

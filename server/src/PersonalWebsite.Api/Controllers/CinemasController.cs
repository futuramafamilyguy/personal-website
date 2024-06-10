using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CinemasController : ControllerBase
{
    private readonly ICinemaService _cinemaService;
    private readonly IPictureCinemaOrchestrator _pictureCinemaOrchestrator;

    public CinemasController(ICinemaService cinemaService, IPictureCinemaOrchestrator pictureCinemaOrchestrator)
    {
        _cinemaService = cinemaService;
        _pictureCinemaOrchestrator = pictureCinemaOrchestrator;
    }

    [HttpGet("")]
    public async Task<ActionResult> GetCinemasAsync()
    {
        var cinemas = await _cinemaService.GetCinemasAsync();

        return Ok(cinemas);
    }

    [HttpPost("")]
    public async Task<ActionResult> CreateCinemaAsync([FromBody]CreateCinemaRequest request)
    {
        if (request is null) return BadRequest("Cinema data is null");

        var cinema = await _cinemaService.AddCinemaAsync(request.Name, request.City);

        return Ok(cinema);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCinemaAsync(string id, [FromBody]CreateCinemaRequest request)
    {
        if (request is null) return BadRequest("Cinema data is null");

        var updatedCinema = await _pictureCinemaOrchestrator.UpdateCinemaAndAssociatedPicturesAsync(id, request.Name, request.City);

        return Ok(updatedCinema);
    }

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
            return Conflict(new { message = ex.Message });
        }
    }
}

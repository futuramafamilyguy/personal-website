using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PicturesController : ControllerBase
{
    private readonly IPictureService _pictureService;
    private readonly IPictureCinemaOrchestrator _pictureCinemaOrchestrator;

    public PicturesController(
        IPictureService pictureTrackingService,
        IPictureCinemaOrchestrator pictureCinemaOrchestrator)
    {
        _pictureService = pictureTrackingService;
        _pictureCinemaOrchestrator = pictureCinemaOrchestrator;
    }

    [HttpGet("{year}")]
    public async Task<ActionResult> GetPicturesAsync(int year, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = int.MaxValue)
    {
        var pictures = await _pictureService.GetPicturesAsync(year, pageNumber, pageSize);

        return Ok(new { pictures.Pictures, pictures.TotalCount });
    }

    [HttpGet("{year}/favorites")]
    public async Task<ActionResult> GetFavoritePicturesAsync(int year)
    {
        var pictures = await _pictureService.GetFavoritePicturesAsync(year);

        return Ok(pictures);
    }

    [HttpPost("{year}")]
    public async Task<ActionResult> CreatePictureAsync(int year, [FromBody]CreatePictureRequest request)
    {
        if (request is null) return BadRequest("Picture data is null");

        var picture = await _pictureCinemaOrchestrator.AddPictureWithCinemaAsync(request.Name, year, request.CinemaId, request.Zinger, request.Alias);

        return Ok(picture);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePictureAsync(string id, [FromBody]UpdatePictureRequest request)
    {
        if (request is null) return BadRequest("Picture data is null");

        var picture = await _pictureCinemaOrchestrator.UpdatePictureWithCinemaAsync(id, request.Name, request.Year, request.CinemaId, request.Zinger, request.Alias);

        return Ok(picture);
    }

    [HttpPut("{year}/{id}/favorite")]
    public async Task<ActionResult> ToggleFavoriteAsync(string id)
    {
        var picture = await _pictureService.ToggleFavoriteAsync(id);

        return Ok(picture);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePictureAsync(string id)
    {
        await _pictureService.RemovePictureAsync(id);

        return NoContent();
    }

    [HttpGet("active-years")]
    public async Task<ActionResult> GetActiveYearsAsync()
    {
        var activeYears = await _pictureService.GetActiveYearsAsync();

        return Ok(activeYears);
    }
}

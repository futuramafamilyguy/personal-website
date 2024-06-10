using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PicturesController : ControllerBase
{
    private readonly IPictureService _pictureTrackingService;
    private readonly IPictureCinemaOrchestrator _pictureCinemaOrchestrator;

    public PicturesController(
        IPictureService pictureTrackingService,
        IPictureCinemaOrchestrator pictureCinemaOrchestrator)
    {
        _pictureTrackingService = pictureTrackingService;
        _pictureCinemaOrchestrator = pictureCinemaOrchestrator;
    }

    [HttpGet("{year}")]
    public async Task<ActionResult> GetPicturesAsync(int year)
    {
        var pictures = await _pictureTrackingService.GetPicturesAsync(year);

        return Ok(pictures);
    }

    [HttpGet("{year}/favorites")]
    public async Task<ActionResult> GetFavoritePicturesAsync(int year)
    {
        var pictures = await _pictureTrackingService.GetFavoritePicturesAsync(year);

        return Ok(pictures);
    }

    [HttpPost("{year}")]
    public async Task<ActionResult> CreatePictureAsync(int year, [FromBody]CreatePictureRequest request)
    {
        if (request is null) return BadRequest("Picture data is null");

        var picture = await _pictureCinemaOrchestrator.AddPictureWithCinemaAsync(request.Name, year, request.CinemaId, request.Zinger);

        return Ok(picture);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePictureAsync(string id, [FromBody]UpdatePictureRequest request)
    {
        if (request is null) return BadRequest("Picture data is null");

        var picture = await _pictureCinemaOrchestrator.UpdatePictureWithCinemaAsync(id, request.Name, request.Year, request.CinemaId, request.Zinger);

        return Ok(picture);
    }

    [HttpPut("{year}/{id}/favorite")]
    public async Task<ActionResult> ToggleFavoriteAsync(string id)
    {
        var picture = await _pictureTrackingService.ToggleFavoriteAsync(id);

        return Ok(picture);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePictureAsync(string id)
    {
        await _pictureTrackingService.RemovePictureAsync(id);

        return NoContent();
    }
}

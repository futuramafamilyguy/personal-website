using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.ImageStorage;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PicturesController : ControllerBase
{
    private readonly IPictureService _pictureService;
    private readonly IPictureCinemaOrchestrator _pictureCinemaOrchestrator;
    private readonly ImageStorageConfiguration _imageStorageConfiguration;
    private readonly ILogger<PicturesController> _logger;

    public PicturesController(
        IPictureService pictureTrackingService,
        IPictureCinemaOrchestrator pictureCinemaOrchestrator,
        IOptions<ImageStorageConfiguration> imageStorageConfiguration,
        ILogger<PicturesController> logger
    )
    {
        _pictureService = pictureTrackingService;
        _pictureCinemaOrchestrator = pictureCinemaOrchestrator;
        _imageStorageConfiguration = imageStorageConfiguration.Value;
        _logger = logger;
    }

    [HttpGet("{year}")]
    public async Task<IActionResult> GetPicturesAsync(
        int year,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = int.MaxValue
    )
    {
        var pictures = await _pictureService.GetPicturesAsync(year, pageNumber, pageSize);

        return Ok(new { pictures.Pictures, pictures.TotalCount });
    }

    // this endpoint is just here for testing purposes and isn't used in prod
    // it's mostly here so postman scripts can populate existing picture fields to avoid typing everything out when testing update picture endpoint
    [HttpGet("{year}/{id}")]
    public async Task<IActionResult> GetPictureAsync(string id)
    {
        var picture = await _pictureService.GetPictureAsync(id);

        return Ok(picture);
    }

    [HttpGet("{year}/favorites")]
    public async Task<IActionResult> GetFavoritePicturesAsync(int year)
    {
        var pictures = await _pictureService.GetFavoritePicturesAsync(year);

        return Ok(pictures);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("{year}")]
    public async Task<IActionResult> CreatePictureAsync(
        int year,
        [FromBody] CreatePictureRequest request
    )
    {
        if (request is null)
            return BadRequest("Picture data is null");
        try
        {
            var picture = await _pictureCinemaOrchestrator.AddPictureWithCinemaAsync(
                request.Name,
                year,
                request.MonthWatched ?? (Month)DateTime.Now.Month,
                request.CinemaId,
                request.YearReleased ?? DateTime.Now.Year,
                request.Zinger,
                request.Alias,
                request.IsFavorite ?? false,
                request.IsKino ?? false,
                request.IsNewRelease ?? true
            );

            return Ok(picture);
        }
        catch (ArgumentException)
        {
            return BadRequest("Invalid arguments for create picture request");
        }
        catch (ValidationException)
        {
            return BadRequest("Picture failed validation checks");
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePictureAsync(
        string id,
        [FromBody] UpdatePictureRequest request
    )
    {
        if (request is null)
            return BadRequest("Picture data is null");

        try
        {
            var picture = await _pictureCinemaOrchestrator.UpdatePictureWithCinemaAsync(
                id,
                request.Name,
                request.YearWatched,
                request.MonthWatched ?? (Month)DateTime.Now.Month,
                request.CinemaId,
                request.YearReleased ?? DateTime.Now.Year,
                request.Zinger,
                request.Alias,
                request.ImageUrl,
                request.ImageObjectKey,
                request.AltImageUrl,
                request.AltImageObjectKey,
                request.IsFavorite ?? false,
                request.IsKino ?? false,
                request.IsNewRelease ?? true
            );

            return Ok(picture);
        }
        catch (ArgumentException)
        {
            return BadRequest("Invalid arguments for update picture request");
        }
        catch (ValidationException)
        {
            return BadRequest("Picture failed validation checks");
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePictureAsync(string id)
    {
        await _pictureService.RemovePictureAsync(id);

        return NoContent();
    }

    [HttpGet("active-years")]
    public async Task<IActionResult> GetActiveYearsAsync()
    {
        var activeYears = await _pictureService.GetActiveYearsAsync();

        return Ok(new { ActiveYears = activeYears });
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("{id}/image-url")]
    public async Task<IActionResult> GenerateImageUploadUrlAsync(
        string id,
        [FromBody] GenerateImageUploadUrlRequest request
    )
    {
        var uploadUrl = await _pictureService.HandleImageUploadAsync(
            id,
            _imageStorageConfiguration.BasePathPicture,
            request.FileExtension,
            request.IsAlt ?? false
        );

        return Ok(new { PresignedUploadUrl = uploadUrl });
    }
}

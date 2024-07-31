using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Images;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PicturesController : ControllerBase
{
    private readonly IPictureService _pictureService;
    private readonly IPictureCinemaOrchestrator _pictureCinemaOrchestrator;
    private readonly IImageStorage _imageStorage;
    private readonly ILogger<PicturesController> _logger;

    public PicturesController(
        IPictureService pictureTrackingService,
        IPictureCinemaOrchestrator pictureCinemaOrchestrator,
        IImageStorage imageStorage,
        ILogger<PicturesController> logger
    )
    {
        _pictureService = pictureTrackingService;
        _pictureCinemaOrchestrator = pictureCinemaOrchestrator;
        _imageStorage = imageStorage;
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

        var picture = await _pictureCinemaOrchestrator.AddPictureWithCinemaAsync(
            request.Name,
            year,
            request.MonthWatched ?? (Month)DateTime.Now.Month,
            request.CinemaId,
            request.YearReleased ?? DateTime.Now.Year,
            request.Zinger,
            request.Alias,
            request.IsFavorite ?? false
        );

        return Ok(picture);
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

        var picture = await _pictureCinemaOrchestrator.UpdatePictureWithCinemaAsync(
            id,
            request.Name,
            request.YearWatched,
            request.MonthWatched,
            request.CinemaId,
            request.YearReleased,
            request.Zinger,
            request.Alias,
            request.ImageUrl,
            request.IsFavorite
        );

        return Ok(picture);
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
    [HttpPost("{pictureId}/image")]
    public async Task<IActionResult> UploadImageAsync(string pictureId, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            _logger.LogError("No file uploaded or file is empty");
            return BadRequest("No file uploaded or file is empty");
        }

        try
        {
            using var stream = imageFile.OpenReadStream();
            var fileExtension = Path.GetExtension(imageFile.FileName);
            var uniqueFileName = $"{pictureId}{fileExtension}";
            await _imageStorage.SaveImageAsync(stream, uniqueFileName, ImageCategory.Picture);

            var imageUrl = _imageStorage.GetImageUrl(uniqueFileName, ImageCategory.Picture);

            return Ok(new { ImageUrl = imageUrl });
        }
        catch (InvalidImageFormatException ex)
        {
            _logger.LogError(ex, $"{imageFile.FileName} has an invalid file extension");
            return BadRequest(
                $"{Path.GetExtension(imageFile.FileName)} is an invalid image format"
            );
        }
        catch (ImageStorageException ex)
        {
            _logger.LogError(
                ex,
                $"An error occurred while saving the image {imageFile.FileName} for picture {pictureId}"
            );
            return StatusCode(500, "An error occurred while saving the image");
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{pictureId}/image")]
    public async Task<IActionResult> DeleteImageAsync(string pictureId)
    {
        var picture = await _pictureService.GetPictureAsync(pictureId);
        var imageFileName = _imageStorage.GetImageFileNameFromUrl(picture.ImageUrl!);

        try
        {
            await _imageStorage.RemoveImageAsync(imageFileName, ImageCategory.Picture);

            return NoContent();
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogError(ex, $"Cannot find image at {picture.ImageUrl}");
            return NotFound($"No image found for picture {pictureId}");
        }
    }
}

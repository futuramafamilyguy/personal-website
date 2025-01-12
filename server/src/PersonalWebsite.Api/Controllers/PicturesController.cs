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
                request.AltImageUrl,
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
    [HttpPost("{id}/image")]
    public async Task<IActionResult> UploadImagesAsync(
        string id,
        IFormFile? imageFile = null,
        IFormFile? altImageFile = null
    )
    {
        try
        {
            using var stream = imageFile?.OpenReadStream();
            var extension = imageFile != null ? Path.GetExtension(imageFile.FileName) : null;

            using var altStream = altImageFile?.OpenReadStream();
            var altExtension =
                altImageFile != null ? Path.GetExtension(altImageFile.FileName) : null;

            if (stream == null && altStream == null)
            {
                return BadRequest("At least one image file is required");
            }

            var imageUrlPair = await _pictureService.UploadPictureImagesAsync(
                stream,
                altStream,
                id,
                extension,
                _imageStorageConfiguration.PictureImageDirectory,
                altExtension
            );

            return Ok(
                new UploadImagesResponse
                {
                    ImageUrl = imageUrlPair.ImageUrl,
                    AltImageUrl = imageUrlPair.AltImageUrl
                }
            );
        }
        catch (ValidationException)
        {
            return BadRequest("Image failed validation checks");
        }
        catch (StorageException)
        {
            return StatusCode(500, "An error occurred while uploading the image");
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id}/image")]
    public async Task<IActionResult> DeleteImagesAsync(
        string id,
        bool deleteImage = false,
        bool deleteAltImage = false
    )
    {
        try
        {
            await _pictureService.DeletePictureImagesAsync(
                id,
                _imageStorageConfiguration.PictureImageDirectory,
                deleteImage,
                deleteAltImage
            );

            return NoContent();
        }
        catch (ValidationException)
        {
            return BadRequest("Picture image failed validation checks");
        }
        catch (StorageException)
        {
            return StatusCode(500, "An error occurred while deleting the image");
        }
    }
}

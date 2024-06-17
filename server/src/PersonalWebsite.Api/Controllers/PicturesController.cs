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

    public PicturesController(
        IPictureService pictureTrackingService,
        IPictureCinemaOrchestrator pictureCinemaOrchestrator,
        IImageStorage imageStorage)
    {
        _pictureService = pictureTrackingService;
        _pictureCinemaOrchestrator = pictureCinemaOrchestrator;
        _imageStorage = imageStorage;
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

        var picture = await _pictureCinemaOrchestrator
            .UpdatePictureWithCinemaAsync(id, request.Name, request.Year, request.CinemaId, request.Zinger, request.Alias, request.ImageUrl);

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

        return Ok(new { ActiveYears = activeYears });
    }

    [HttpPost("{pictureId}/image")]
    public async Task<ActionResult> UploadImageAsync(string pictureId, IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
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
        catch (InvalidImageFormatException)
        {
            return BadRequest($"{Path.GetExtension(imageFile.FileName)} is an invalid image format");
        }
        catch (ImageStorageException)
        {
            return StatusCode(500, "An error occurred while saving the image");
        }
    }

    [HttpDelete("{pictureId}/image")]
    public async Task<ActionResult> DeleteImageAsync(string pictureId)
    {
        var picture = await _pictureService.GetPictureAsync(pictureId);
        var imageFileName = _imageStorage.GetImageFileNameFromUrl(picture.ImageUrl);

        try
        {
            await _imageStorage.RemoveImageAsync(imageFileName, ImageCategory.Picture);

            return NoContent();
        }
        catch (FileNotFoundException)
        {
            return NotFound($"No image found for picture {pictureId}");
        }
    }
}

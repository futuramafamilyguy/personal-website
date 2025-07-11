using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.ImageStorage;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IMovieCinemaOrchestrator _movieCinemaOrchestrator;
    private readonly ImageStorageConfiguration _imageStorageConfiguration;

    public MoviesController(
        IMovieService movieService,
        IMovieCinemaOrchestrator movieCinemaOrchestrator,
        IOptions<ImageStorageConfiguration> imageStorageConfiguration
    )
    {
        _movieService = movieService;
        _movieCinemaOrchestrator = movieCinemaOrchestrator;
        _imageStorageConfiguration = imageStorageConfiguration.Value;
    }

    [HttpGet("{year}")]
    public async Task<IActionResult> GetMoviesAsync(
        int year,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = int.MaxValue
    )
    {
        var movies = await _movieService.GetMoviesAsync(year, pageNumber, pageSize);

        return Ok(new { movies.Movies, movies.TotalCount });
    }

    // this endpoint is just here for testing purposes and isn't used in prod
    // it's mostly here so postman scripts can populate existing movie fields to avoid typing everything out when testing update movie endpoint
    [HttpGet("{year}/{id}")]
    public async Task<IActionResult> GetMovieAsync(string id)
    {
        var movie = await _movieService.GetMovieAsync(id);

        return Ok(movie);
    }

    [HttpGet("{year}/nominees")]
    public async Task<IActionResult> GetNomineesAsync(int year)
    {
        var movies = await _movieService.GetNomineesAsync(year);

        return Ok(movies);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("{year}")]
    public async Task<IActionResult> CreateMovieAsync(
        int year,
        [FromBody] CreateMovieRequest request
    )
    {
        var movie = await _movieCinemaOrchestrator.AddMovieWithCinemaAsync(
            request.Name,
            year,
            request.Month ?? (Month)DateTime.Now.Month,
            request.CinemaId,
            request.ReleaseYear ?? DateTime.Now.Year,
            request.Zinger,
            request.Alias,
            request.IsNominated ?? false,
            request.IsKino ?? false,
            request.IsRetro ?? true
        );

        return Ok(movie);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovieAsync(
        string id,
        [FromBody] UpdateMovieRequest request
    )
    {
        var movie = await _movieCinemaOrchestrator.UpdateMovieWithCinemaAsync(
            id,
            request.Name,
            request.Year,
            request.Month ?? (Month)DateTime.Now.Month,
            request.CinemaId,
            request.ReleaseYear ?? DateTime.Now.Year,
            request.Zinger,
            request.Alias,
            request.ImageUrl,
            request.ImageObjectKey,
            request.AltImageUrl,
            request.AltImageObjectKey,
            request.IsNominated ?? false,
            request.IsKino ?? false,
            request.IsRetro ?? true
        );

        return Ok(movie);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovieAsync(string id)
    {
        await _movieService.RemoveMovieAsync(id);

        return NoContent();
    }

    [HttpGet("active-years")]
    public async Task<IActionResult> GetActiveYearsAsync()
    {
        var activeYears = await _movieService.GetActiveYearsAsync();

        return Ok(new { ActiveYears = activeYears });
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("{id}/image-url")]
    public async Task<IActionResult> GenerateImageUploadUrlAsync(
        string id,
        [FromBody] GenerateImageUploadUrlRequest request
    )
    {
        var uploadUrl = await _movieService.HandleImageUploadAsync(
            id,
            _imageStorageConfiguration.BasePathMovie,
            request.FileExtension,
            request.IsAlt ?? false
        );

        return Ok(new { PresignedUploadUrl = uploadUrl });
    }
}

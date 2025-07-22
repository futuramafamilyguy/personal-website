using Microsoft.Extensions.Logging;
using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class MovieCinemaOrchestrator : IMovieCinemaOrchestrator
{
    private readonly IMovieService _movieService;
    private readonly ICinemaService _cinemaService;
    private readonly ILogger<MovieCinemaOrchestrator> _logger;

    public MovieCinemaOrchestrator(
        IMovieService movieService,
        ICinemaService cinemaService,
        ILogger<MovieCinemaOrchestrator> logger
    )
    {
        _movieService = movieService;
        _cinemaService = cinemaService;
        _logger = logger;
    }

    public async Task<Movie> AddMovieWithCinemaAsync(
        string name,
        int year,
        Month month,
        string cinemaId,
        int releaseYear,
        string? zinger,
        string? alias,
        bool isNominated,
        bool isKino,
        bool isRetro
    )
    {
        var cinema = await _cinemaService.GetCinemaAsync(cinemaId);
        var movie = await _movieService.AddMovieAsync(
            name,
            year,
            month,
            cinema,
            releaseYear,
            zinger,
            alias,
            isNominated,
            isKino,
            isRetro
        );

        return movie;
    }

    public async Task<Movie> UpdateMovieWithCinemaAsync(
        string movieId,
        string name,
        int year,
        Month month,
        string cinemaId,
        int releaseYear,
        string? zinger,
        string? alias,
        string? imageUrl,
        string? imageObjectKey,
        string? altImageUrl,
        string? altImageObjectKey,
        bool isNominated,
        bool isKino,
        bool isRetro
    )
    {
        var cinema = await _cinemaService.GetCinemaAsync(cinemaId);
        var updatedMovie = await _movieService.UpdateMovieAsync(
            movieId,
            name,
            year,
            month,
            cinema,
            releaseYear,
            zinger,
            alias,
            imageUrl,
            imageObjectKey,
            altImageUrl,
            altImageObjectKey,
            isNominated,
            isKino,
            isRetro
        );

        return updatedMovie;
    }

    public async Task<Cinema> UpdateCinemaAndAssociatedMoviesAsync(
        string cinemaId,
        string cinemaName,
        string city
    )
    {
        var updatedCinema = await _cinemaService.UpdateCinemaAsync(cinemaId, cinemaName, city);
        var updatedCount = await _movieService.UpdateCinemaOfMoviesAsync(cinemaId, updatedCinema);
        _logger.LogInformation(
            $"updated cinema info of {updatedCount} movies that are associated with cinema {cinemaId}"
        );

        return updatedCinema;
    }

    public async Task ValidateAndDeleteCinema(string cinemaId)
    {
        var cinemaExists = await _movieService.CheckIfAnyMoviesAssociatedWithCinemaAsync(cinemaId);

        if (cinemaExists)
            throw new DomainValidationException(
                $"cinema cannot be deleted because one or more movies are still associated with the cinema: {cinemaId}"
            );

        await _cinemaService.RemoveCinemaAsync(cinemaId);
    }
}

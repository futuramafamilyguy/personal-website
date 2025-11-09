using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

/// <summary>
/// Facilitates interactions between <see cref="IMovieService"/> and <see cref="ICinemaService"/>.
/// </summary>
/// <remarks>
/// Some operations on movies requires interactions with cinemas and vice versa.
/// Introduce an orchestration class to serve as a bridge to avoid a circular dependency.
/// </remarks>
public interface IMovieCinemaOrchestrator
{
    Task<Movie> AddMovieWithCinemaAsync(
        string name,
        int year,
        Month month,
        string cinemaId,
        int releaseYear,
        string? zinger,
        string? alias,
        string? motif,
        bool isNominated,
        bool isKino,
        bool isRetro
    );
    Task<Movie> UpdateMovieWithCinemaAsync(
        string id,
        string name,
        int year,
        Month month,
        string cinemaId,
        int releaseYear,
        string? zinger,
        string? alias,
        string? motif,
        string? imageUrl,
        string? imageObjectKey,
        string? altImageUrl,
        string? altImageObjectKey,
        bool isNominated,
        bool isKino,
        bool isRetro
    );
    Task<Cinema> UpdateCinemaAndAssociatedMoviesAsync(
        string cinemaId,
        string cinemaName,
        string city
    );
    Task ValidateAndDeleteCinema(string cinemaId);
}

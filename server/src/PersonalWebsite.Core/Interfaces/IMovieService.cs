using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IMovieService
{
    Task<(IEnumerable<Movie> Movies, long TotalCount)> GetMoviesAsync(
        int year,
        int pageNumber,
        int pageSize
    );
    Task<Movie> GetMovieAsync(string id);
    Task<IEnumerable<Movie>> GetNomineesAsync(int year);
    Task<Movie> AddMovieAsync(
        string name,
        int year,
        Month month,
        Cinema cinema,
        int releaseYear,
        string? zinger,
        string? alias,
        bool IsNominated,
        bool isKino,
        bool isRetro
    );
    Task<Movie> UpdateMovieAsync(
        string id,
        string name,
        int year,
        Month month,
        Cinema cinema,
        int releaseYear,
        string? zinger,
        string? alias,
        string? imageUrl,
        string? imageObjectKey,
        string? altImageUrl,
        string? altImageObjectKey,
        bool IsNominated,
        bool isKino,
        bool isRetro
    );
    Task<long> UpdateCinemaOfMoviesAsync(string cinemaId, Cinema cinema);
    Task RemoveMovieAsync(string id);
    Task<bool> CheckIfAnyMoviesAssociatedWithCinemaAsync(string id);
    Task<IEnumerable<int>> GetActiveYearsAsync();
    Task<string> HandleImageUploadAsync(
        string id,
        string imageBasePath,
        string fileExtension,
        bool isAlt
    );
}

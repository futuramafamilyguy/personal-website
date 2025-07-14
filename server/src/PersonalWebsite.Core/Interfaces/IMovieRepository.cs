using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IMovieRepository
{
    Task<Movie?> GetAsync(string id);
    Task<(IEnumerable<Movie> Movies, long TotalCount)> GetByYearAsync(
        int year,
        int pageNumber,
        int pageSize
    );
    Task<IEnumerable<Movie>> GetNomineesByYearAsync(int year);
    Task<Movie> AddAsync(Movie movie);
    Task<bool> UpdateAsync(string id, Movie movie);
    Task<long> UpdateCinemaInfoAsync(string cinemaId, Cinema updatedCinema);
    Task RemoveAsync(string id);
    Task<bool> CheckCinemaAssociationExistenceAsync(string cinemaId);
    Task<IEnumerable<int>> GetActiveYearsAsync();
    Task<bool> CheckKinoExistenceAsync(int year, string? id = null);
    Task UpdateImageInfoAsync(string id, string imageObjectKey, string imageUrl, bool isAlt);
}

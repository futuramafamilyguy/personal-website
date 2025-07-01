using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPictureRepository
{
    Task<Picture?> GetAsync(string id);
    Task<(IEnumerable<Picture> Pictures, long TotalCount)> GetByYearWatchedAsync(
        int yearWatched,
        int pageNumber,
        int pageSize
    );
    Task<IEnumerable<Picture>> GetFavoritesByYearWatchedAsync(int yearWatched);
    Task<Picture> AddAsync(Picture picture);
    Task<bool> UpdateAsync(string id, Picture picture);
    Task<long> UpdateCinemaInfoAsync(string cinemaId, Cinema updatedCinema);
    Task RemoveAsync(string id);
    Task<bool> CheckCinemaAssociationExistenceAsync(string cinemaId);
    Task<IEnumerable<int>> GetActiveYearsAsync();
    Task<bool> CheckKinoPictureExistenceAsync(int year, string? id = null);
    Task UpdateImageInfoAsync(string id, string imageObjectKey, string imageUrl, bool isAlt);
}

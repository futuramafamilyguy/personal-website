using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPictureRepository
{
    Task<Picture> GetAsync(string id);
    Task<(IEnumerable<Picture> Pictures, long TotalCount)> GetByYearAsync(int year, int pageNumber, int pageSize);
    Task<IEnumerable<Picture>> GetFavoriteByYearAsync(int year);
    Task<Picture> AddAsync(Picture picture);
    Task<Picture> UpdateAsync(string id, Picture picture);
    Task<Picture> ToggleFavoriteStatusAsync(string id);
    Task UpdateCinemaInfoAsync(string cinemaId, Cinema updatedCinema);
    Task RemoveAsync(string id);
    Task<bool> CheckCinemaAssociationExistenceAsync(string cinemaId);
    Task<IEnumerable<int>> GetActiveYearsAsync();
}

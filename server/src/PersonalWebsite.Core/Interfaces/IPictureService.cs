using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPictureService
{
    Task<(IEnumerable<Picture> Pictures, long TotalCount)> GetPicturesAsync(
        int yearWatched,
        int pageNumber,
        int pageSize
    );
    Task<Picture> GetPictureAsync(string id);
    Task<IEnumerable<Picture>> GetFavoritePicturesAsync(int yearWatched);
    Task<Picture> AddPictureAsync(
        string name,
        int yearWatched,
        Month monthWatched,
        Cinema cinema,
        string? zinger,
        string? alias
    );
    Task<Picture> UpdatePictureAsync(
        string id,
        string name,
        int yearWatched,
        Month monthWatched,
        Cinema cinema,
        string? zinger,
        string? alias,
        string? imageUrl
    );
    Task<Picture> ToggleFavoriteAsync(string id);
    Task<long> UpdateCinemaOfPicturesAsync(string cinemaId, Cinema cinema);
    Task RemovePictureAsync(string id);
    Task<bool> CheckIfAnyPicturesAssociatedWithCinemaAsync(string id);
    Task<IEnumerable<int>> GetActiveYearsAsync();
}

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
        int yearReleased,
        string? zinger,
        string? alias,
        bool isFavorite
    );
    Task<Picture> UpdatePictureAsync(
        string id,
        string name,
        int yearWatched,
        Month monthWatched,
        Cinema cinema,
        int yearReleased,
        string? zinger,
        string? alias,
        string? imageUrl,
        bool isFavorite
    );
    Task<long> UpdateCinemaOfPicturesAsync(string cinemaId, Cinema cinema);
    Task RemovePictureAsync(string id);
    Task<bool> CheckIfAnyPicturesAssociatedWithCinemaAsync(string id);
    Task<IEnumerable<int>> GetActiveYearsAsync();
    Task<string> UploadPictureImageAsync(
        Stream imageStream,
        string id,
        string imageExtension,
        string imageDirectory
    );
    Task DeletePictureImageAsync(string id, string imageDirectory);
}

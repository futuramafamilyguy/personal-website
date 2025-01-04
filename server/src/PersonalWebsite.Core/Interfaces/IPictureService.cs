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
        bool isFavorite,
        bool isNewRelease
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
        string? altImageUrl,
        bool isFavorite,
        bool isNewRelease
    );
    Task<long> UpdateCinemaOfPicturesAsync(string cinemaId, Cinema cinema);
    Task RemovePictureAsync(string id);
    Task<bool> CheckIfAnyPicturesAssociatedWithCinemaAsync(string id);
    Task<IEnumerable<int>> GetActiveYearsAsync();
    Task<(string? ImageUrl, string? AltImageUrl)> UploadPictureImagesAsync(
        Stream? imageStream,
        Stream? altImageStream,
        string id,
        string? imageExtension,
        string imageDirectory,
        string? altImageExtension
    );
    Task DeletePictureImagesAsync(
        string id,
        string imageDirectory,
        bool deleteImage,
        bool deleteAltImage
    );
}

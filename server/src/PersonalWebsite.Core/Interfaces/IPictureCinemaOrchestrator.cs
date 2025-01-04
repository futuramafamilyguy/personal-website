using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

/// <summary>
/// Facilitates interactions between <see cref="IPictureService"/> and <see cref="ICinemaService"/>.
/// </summary>
/// <remarks>
/// Some operations on pictures requires interactions with cinemas and vice versa.
/// Introduce an orchestration class to serve as a bridge to avoid a circular dependency.
/// </remarks>
public interface IPictureCinemaOrchestrator
{
    Task<Picture> AddPictureWithCinemaAsync(
        string pictureName,
        int yearWatched,
        Month monthWatched,
        string cinemaId,
        int yearReleased,
        string? zinger,
        string? alias,
        bool isFavorite,
        bool isNewRelease
    );
    Task<Picture> UpdatePictureWithCinemaAsync(
        string pictureId,
        string pictureName,
        int yearWatched,
        Month monthWatched,
        string cinemaId,
        int yearReleased,
        string? zinger,
        string? alias,
        string? imageUrl,
        string? altImageUrl,
        bool isFavorite,
        bool isNewRelease
    );
    Task<Cinema> UpdateCinemaAndAssociatedPicturesAsync(
        string cinemaId,
        string cinemaName,
        string city
    );
    Task ValidateAndDeleteCinema(string cinemaId);
}

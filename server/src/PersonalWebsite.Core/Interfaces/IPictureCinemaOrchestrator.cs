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
    Task<Picture> AddPictureWithCinemaAsync(string pictureName, int year, string cinemaId, string? zinger, string? alias);
    Task<Picture> UpdatePictureWithCinemaAsync(string pictureId, string pictureName, int year, string cinemaId, string? zinger, string? alias, string? imageUrl);
    Task<Cinema> UpdateCinemaAndAssociatedPicturesAsync(string cinemaId, string cinemaName, string city);
    Task ValidateAndDeleteCinema(string cinemaId);
}

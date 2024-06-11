using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPictureService
{
    Task<IEnumerable<Picture>> GetPicturesAsync(int year);
    Task<IEnumerable<Picture>> GetFavoritePicturesAsync(int year);
    Task<Picture> AddPictureAsync(string name, int year, Cinema cinema, string? zinger);
    Task<Picture> UpdatePictureAsync(string id, string name, int year, Cinema cinema, string? zinger);
    Task<Picture> ToggleFavoriteAsync(string id);
    Task UpdateCinemaOfPicturesAsync(string cinemaId, Cinema cinema);
    Task RemovePictureAsync(string id);
    Task<bool> CheckIfAnyPicturesAssociatedWithCinemaAsync(string id);
    Task<IEnumerable<int>> GetActiveYearsAsync();
}

using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class PictureService : IPictureService
{
    private readonly IPictureRepository _pictureRepository;

    public PictureService(IPictureRepository pictureRepository)
    {
        _pictureRepository = pictureRepository;
    }

    public async Task<(IEnumerable<Picture> Pictures, long TotalCount)> GetPicturesAsync(
        int yearWatched,
        int pageNumber,
        int pageSize
    ) => await _pictureRepository.GetByYearWatchedAsync(yearWatched, pageNumber, pageSize);

    public async Task<Picture> GetPictureAsync(string id) => await _pictureRepository.GetAsync(id);

    public async Task<IEnumerable<Picture>> GetFavoritePicturesAsync(int yearWatched) =>
        await _pictureRepository.GetFavoritesByYearWatchedAsync(yearWatched);

    public async Task<Picture> AddPictureAsync(
        string name,
        int yearWatched,
        Month monthWatched,
        Cinema cinema,
        int yearReleased,
        string? zinger,
        string? alias
    )
    {
        var picture = await _pictureRepository.AddAsync(
            new Picture
            {
                Name = name,
                YearWatched = yearWatched,
                MonthWatched = monthWatched,
                Cinema = cinema,
                YearReleased = yearReleased,
                Zinger = zinger,
                Alias = alias
            }
        );

        return picture;
    }

    public async Task<Picture> UpdatePictureAsync(
        string id,
        string name,
        int yearWatched,
        Month monthWatched,
        Cinema cinema,
        int yearReleased,
        string? zinger,
        string? alias,
        string? imageUrl
    )
    {
        var updatedPicture = await _pictureRepository.UpdateAsync(
            id,
            new Picture
            {
                Id = id,
                Name = name,
                YearWatched = yearWatched,
                MonthWatched = monthWatched,
                Cinema = cinema,
                YearReleased = yearReleased,
                Zinger = zinger,
                Alias = alias,
                ImageUrl = imageUrl
            }
        );

        return updatedPicture;
    }

    public async Task<Picture> ToggleFavoriteAsync(string id)
    {
        var updatedPicture = await _pictureRepository.ToggleFavoriteStatusAsync(id);

        return updatedPicture;
    }

    public async Task<long> UpdateCinemaOfPicturesAsync(string cinemaId, Cinema cinema) =>
        await _pictureRepository.UpdateCinemaInfoAsync(cinemaId, cinema);

    public async Task RemovePictureAsync(string id) => await _pictureRepository.RemoveAsync(id);

    public async Task<bool> CheckIfAnyPicturesAssociatedWithCinemaAsync(string cinemaId) =>
        await _pictureRepository.CheckCinemaAssociationExistenceAsync(cinemaId);

    public async Task<IEnumerable<int>> GetActiveYearsAsync() =>
        await _pictureRepository.GetActiveYearsAsync();
}

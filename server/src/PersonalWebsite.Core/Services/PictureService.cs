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

    public async Task<(IEnumerable<Picture> Pictures, long TotalCount)> GetPicturesAsync(int year, int pageNumber, int pageSize) =>
        await _pictureRepository.GetByYearAsync(year, pageNumber, pageSize);

    public async Task<Picture> GetPictureAsync(string id) =>
        await _pictureRepository.GetAsync(id);

    public async Task<IEnumerable<Picture>> GetFavoritePicturesAsync(int year) =>
        await _pictureRepository.GetFavoriteByYearAsync(year);

    public async Task<Picture> AddPictureAsync(string name, int year, Cinema cinema, string? zinger, string? alias)
    {
        var picture = await _pictureRepository.AddAsync(new Picture
        {
            Name = name,
            Year = year,
            Cinema = cinema,
            Zinger = zinger,
            Alias = alias
        });

        return picture;
    }

    public async Task<Picture> UpdatePictureAsync(string id, string name, int year, Cinema cinema, string? zinger, string? alias, string? imageUrl)
    {
        var updatedPicture = await _pictureRepository.UpdateAsync(id, new Picture
        {
            Id = id,
            Name = name,
            Year = year,
            Cinema = cinema,
            Zinger = zinger,
            Alias = alias,
            ImageUrl = imageUrl
        });

        return updatedPicture;
    }

    public async Task<Picture> ToggleFavoriteAsync(string id)
    {
        var updatedPicture = await _pictureRepository.ToggleFavoriteStatusAsync(id);

        return updatedPicture;
    }

    public async Task<long> UpdateCinemaOfPicturesAsync(string cinemaId, Cinema cinema) =>
        await _pictureRepository.UpdateCinemaInfoAsync(cinemaId, cinema);

    public async Task RemovePictureAsync(string id) =>
        await _pictureRepository.RemoveAsync(id);

    public async Task<bool> CheckIfAnyPicturesAssociatedWithCinemaAsync(string cinemaId) =>
        await _pictureRepository.CheckCinemaAssociationExistenceAsync(cinemaId);

    public async Task<IEnumerable<int>> GetActiveYearsAsync() =>
        await _pictureRepository.GetActiveYearsAsync();
}

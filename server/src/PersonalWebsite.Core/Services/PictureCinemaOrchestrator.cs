using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class PictureCinemaOrchestrator : IPictureCinemaOrchestrator
{
    private readonly IPictureService _pictureService;
    private readonly ICinemaService _cinemaService;

    public PictureCinemaOrchestrator(IPictureService pictureService, ICinemaService cinemaService)
    {
        _pictureService = pictureService;
        _cinemaService = cinemaService;
    }

    public async Task<Picture> AddPictureWithCinemaAsync(string pictureName, int year, string cinemaId, string? zinger, string? alias)
    {
        var cinema = await _cinemaService.GetCinemaAsync(cinemaId);
        var picture = await _pictureService.AddPictureAsync(pictureName, year, cinema, zinger, alias);

        return picture;
    }

    public async Task<Picture> UpdatePictureWithCinemaAsync(string pictureId, string pictureName, int year, string cinemaId, string? zinger, string? alias)
    {
        var cinema = await _cinemaService.GetCinemaAsync(cinemaId);
        var updatedPicture = await _pictureService.UpdatePictureAsync(pictureId, pictureName, year, cinema, zinger, alias);

        return updatedPicture;
    }

    public async Task<Cinema> UpdateCinemaAndAssociatedPicturesAsync(string cinemaId, string cinemaName, string city)
    {
        var updatedCinema = await _cinemaService.UpdateCinemaAsync(cinemaId, cinemaName, city);
        await _pictureService.UpdateCinemaOfPicturesAsync(cinemaId, updatedCinema);

        return updatedCinema;
    }

    public async Task ValidateAndDeleteCinema(string cinemaId)
    {
        var cinemaExists = await _pictureService.CheckIfAnyPicturesAssociatedWithCinemaAsync(cinemaId);
        if (cinemaExists)
        {
            throw new InvalidOperationException(
                $"Cinema {cinemaId} cannot be deleted because one or more pictures are still associated with the cinema");
        }

        await _cinemaService.RemoveCinemaAsync(cinemaId);
    }
}

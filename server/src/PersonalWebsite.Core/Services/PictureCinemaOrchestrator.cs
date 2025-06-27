using Microsoft.Extensions.Logging;
using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class PictureCinemaOrchestrator : IPictureCinemaOrchestrator
{
    private readonly IPictureService _pictureService;
    private readonly ICinemaService _cinemaService;
    private readonly ILogger<PictureCinemaOrchestrator> _logger;

    public PictureCinemaOrchestrator(
        IPictureService pictureService,
        ICinemaService cinemaService,
        ILogger<PictureCinemaOrchestrator> logger
    )
    {
        _pictureService = pictureService;
        _cinemaService = cinemaService;
        _logger = logger;
    }

    public async Task<Picture> AddPictureWithCinemaAsync(
        string pictureName,
        int yearWatched,
        Month monthWatched,
        string cinemaId,
        int yearReleased,
        string? zinger,
        string? alias,
        bool isFavorite,
        bool isKino,
        bool isNewRelease
    )
    {
        var cinema = await _cinemaService.GetCinemaAsync(cinemaId);
        var picture = await _pictureService.AddPictureAsync(
            pictureName,
            yearWatched,
            monthWatched,
            cinema,
            yearReleased,
            zinger,
            alias,
            isFavorite,
            isKino,
            isNewRelease
        );

        return picture;
    }

    public async Task<Picture> UpdatePictureWithCinemaAsync(
        string pictureId,
        string pictureName,
        int yearWatched,
        Month monthWatched,
        string cinemaId,
        int yearReleased,
        string? zinger,
        string? alias,
        string? imageUrl,
        string? imageObjectKey,
        string? altImageUrl,
        string? altImageObjectKey,
        bool isFavorite,
        bool isKino,
        bool isNewRelease
    )
    {
        var cinema = await _cinemaService.GetCinemaAsync(cinemaId);
        var updatedPicture = await _pictureService.UpdatePictureAsync(
            pictureId,
            pictureName,
            yearWatched,
            monthWatched,
            cinema,
            yearReleased,
            zinger,
            alias,
            imageUrl,
            imageObjectKey,
            altImageUrl,
            altImageObjectKey,
            isFavorite,
            isKino,
            isNewRelease
        );

        return updatedPicture;
    }

    public async Task<Cinema> UpdateCinemaAndAssociatedPicturesAsync(
        string cinemaId,
        string cinemaName,
        string city
    )
    {
        var updatedCinema = await _cinemaService.UpdateCinemaAsync(cinemaId, cinemaName, city);
        var updatedCount = await _pictureService.UpdateCinemaOfPicturesAsync(
            cinemaId,
            updatedCinema
        );
        _logger.LogInformation(
            $"Updated cinema info of {updatedCount} pictures that are associated with cinema {cinemaId}"
        );

        return updatedCinema;
    }

    public async Task ValidateAndDeleteCinema(string cinemaId)
    {
        var cinemaExists = await _pictureService.CheckIfAnyPicturesAssociatedWithCinemaAsync(
            cinemaId
        );

        if (cinemaExists)
        {
            throw new InvalidOperationException(
                $"Cinema {cinemaId} cannot be deleted because one or more pictures are still associated with the cinema"
            );
        }

        await _cinemaService.RemoveCinemaAsync(cinemaId);
    }
}

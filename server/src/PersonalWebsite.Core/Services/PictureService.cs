using Microsoft.Extensions.Logging;
using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class PictureService : IPictureService
{
    private readonly IPictureRepository _pictureRepository;
    private readonly IImageStorage _imageStorage;
    private readonly ILogger<PictureService> _logger;

    public PictureService(
        IPictureRepository pictureRepository,
        IImageStorage imageStorage,
        ILogger<PictureService> logger
    )
    {
        _pictureRepository = pictureRepository;
        _imageStorage = imageStorage;
        _logger = logger;
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
        string? alias,
        bool isFavorite,
        bool isKino,
        bool isNewRelease
    )
    {
        if (isKino && !isFavorite)
        {
            _logger.LogError($"Picture cannot be KINO if not also a favourite");
            throw new ArgumentException("Failed to create picture due to invalid arguments");
        }

        if (isKino && await _pictureRepository.CheckKinoPictureExistenceAsync(yearWatched))
        {
            _logger.LogError($"{yearWatched} already has a KINO");
            throw new ValidationException(
                "Failed to create picture due to picture validation error"
            );
        }

        var picture = await _pictureRepository.AddAsync(
            new Picture
            {
                Name = name,
                YearWatched = yearWatched,
                MonthWatched = monthWatched,
                Cinema = cinema,
                YearReleased = yearReleased,
                Zinger = zinger,
                Alias = alias,
                IsFavorite = isFavorite,
                IsKino = isKino,
                IsNewRelease = isNewRelease
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
        string? imageUrl,
        string? imageObjectKey,
        string? altImageUrl,
        string? altImageObjectKey,
        bool isFavorite,
        bool isKino,
        bool isNewRelease
    )
    {
        if (isKino && !isFavorite)
        {
            _logger.LogError($"Picture `{id}` cannot be KINO if not also a favourite");
            throw new ArgumentException("Failed to update picture due to invalid arguments");
        }

        if (isKino && await _pictureRepository.CheckKinoPictureExistenceAsync(yearWatched, id))
        {
            _logger.LogError($"{yearWatched} already has a KINO");
            throw new ValidationException(
                "Failed to update picture due to picture validation error"
            );
        }

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
                ImageUrl = imageUrl,
                ImageObjectKey = imageObjectKey,
                AltImageUrl = altImageUrl,
                AltImageObjectKey = altImageObjectKey,
                IsFavorite = isFavorite,
                IsKino = isKino,
                IsNewRelease = isNewRelease
            }
        );

        return updatedPicture;
    }

    public async Task<long> UpdateCinemaOfPicturesAsync(string cinemaId, Cinema cinema) =>
        await _pictureRepository.UpdateCinemaInfoAsync(cinemaId, cinema);

    public async Task RemovePictureAsync(string id)
    {
        var picture = await _pictureRepository.GetAsync(id);
        if (!string.IsNullOrEmpty(picture.ImageObjectKey))
            await _imageStorage.DeleteObjectAsync(picture.ImageObjectKey);
        if (!string.IsNullOrEmpty(picture.AltImageObjectKey))
            await _imageStorage.DeleteObjectAsync(picture.AltImageObjectKey);

        await _pictureRepository.RemoveAsync(id);
    }

    public async Task<bool> CheckIfAnyPicturesAssociatedWithCinemaAsync(string cinemaId) =>
        await _pictureRepository.CheckCinemaAssociationExistenceAsync(cinemaId);

    public async Task<IEnumerable<int>> GetActiveYearsAsync() =>
        await _pictureRepository.GetActiveYearsAsync();

    public async Task<string> HandleImageUploadAsync(
        string id,
        string imageBasePath,
        string fileExtension,
        bool isAlt
    )
    {
        var picture = await _pictureRepository.GetAsync(id);

        if (isAlt)
        {
            if (!string.IsNullOrEmpty(picture.AltImageObjectKey))
                await _imageStorage.DeleteObjectAsync(picture.AltImageObjectKey);
        }
        else
        {
            if (!string.IsNullOrEmpty(picture.ImageObjectKey))
                await _imageStorage.DeleteObjectAsync(picture.ImageObjectKey);
        }

        var objectKey = isAlt
            ? $"{imageBasePath}/{picture.YearWatched}/{id}-alt.{fileExtension}"
            : $"{imageBasePath}/{picture.YearWatched}/{id}.{fileExtension}";
        var presignedUrl = await _imageStorage.GeneratePresignedUploadUrlAsync(
            objectKey,
            TimeSpan.FromMinutes(5)
        );
        var publicUrl = _imageStorage.GetPublicUrl(objectKey);

        await _pictureRepository.UpdateImageInfoAsync(id, objectKey, publicUrl, isAlt);

        return presignedUrl;
    }
}

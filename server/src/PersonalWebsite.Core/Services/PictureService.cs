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
        bool isFavorite
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
                Alias = alias,
                IsFavorite = isFavorite
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
        string? altImageUrl,
        bool isFavorite
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
                ImageUrl = imageUrl,
                AltImageUrl = altImageUrl,
                IsFavorite = isFavorite
            }
        );

        return updatedPicture;
    }

    public async Task<long> UpdateCinemaOfPicturesAsync(string cinemaId, Cinema cinema) =>
        await _pictureRepository.UpdateCinemaInfoAsync(cinemaId, cinema);

    public async Task RemovePictureAsync(string id) => await _pictureRepository.RemoveAsync(id);

    public async Task<bool> CheckIfAnyPicturesAssociatedWithCinemaAsync(string cinemaId) =>
        await _pictureRepository.CheckCinemaAssociationExistenceAsync(cinemaId);

    public async Task<IEnumerable<int>> GetActiveYearsAsync() =>
        await _pictureRepository.GetActiveYearsAsync();

    public async Task<(string? ImageUrl, string? AltImageUrl)> UploadPictureImagesAsync(
        Stream? imageStream,
        Stream? altImageStream,
        string id,
        string? imageExtension,
        string imageDirectory,
        string? altImageExtension
    )
    {
        var picture = await GetPictureAsync(id);

        try
        {
            string? imageUrl = null;
            string? altImageUrl = null;

            if (imageStream is not null)
            {
                var imageName = $"{id}{imageExtension}";
                imageUrl = await PrepareAndSaveImageAsync(
                    picture,
                    imageStream,
                    imageName,
                    imageExtension!,
                    imageDirectory
                );
            }

            if (altImageStream is not null)
            {
                var altImageName = $"alt-{id}{altImageExtension}";
                altImageUrl = await PrepareAndSaveImageAsync(
                    picture,
                    altImageStream,
                    altImageName,
                    altImageExtension!,
                    imageDirectory
                );
            }

            return (imageUrl, altImageUrl);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Image validation failure encountered");
            throw;
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "An error occurred while saving the image");
            throw;
        }
    }

    public async Task DeletePictureImagesAsync(
        string id,
        string imageDirectory,
        bool deleteImage,
        bool deleteAltImage
    )
    {
        var picture = await GetPictureAsync(id);

        try
        {
            if (
                (deleteImage && picture.ImageUrl is null)
                || (deleteAltImage && picture.AltImageUrl is null)
            )
            {
                _logger.LogError($"Picture '{id}' does not have an image that can be deleted");
                throw new ValidationException("No image associated with picture");
            }

            var imageYearDirectory = $"{imageDirectory}/{picture.YearWatched}";

            if (deleteImage)
            {
                var imageFileName = _imageStorage.GetImageFileNameFromUrl(picture.ImageUrl!);
                await _imageStorage.RemoveImageAsync(imageFileName, imageYearDirectory);
            }

            if (deleteAltImage)
            {
                var altImageFileName = _imageStorage.GetImageFileNameFromUrl(picture.AltImageUrl!);
                await _imageStorage.RemoveImageAsync(altImageFileName, imageYearDirectory);
            }
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Image validation failure encountered");
            throw;
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting image");
            throw;
        }
    }

    private async Task<string> PrepareAndSaveImageAsync(
        Picture picture,
        Stream imageStream,
        string imageName,
        string imageExtension,
        string imageDirectory
    )
    {
        if (!_imageStorage.IsValidImageFormat(imageName))
        {
            _logger.LogError($"Image extension '{imageExtension}' not supported");
            throw new ValidationException("Invalid image extension");
        }

        var imageYearDirectory = $"{imageDirectory}/{picture.YearWatched}";

        return await _imageStorage.SaveImageAsync(imageStream, imageName, imageYearDirectory);
    }
}

using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IImageStorage _imageStorage;

    public MovieService(IMovieRepository movieRepository, IImageStorage imageStorage)
    {
        _movieRepository = movieRepository;
        _imageStorage = imageStorage;
    }

    public async Task<(IEnumerable<Movie> Movies, long TotalCount)> GetMoviesAsync(
        int year,
        int pageNumber,
        int pageSize
    ) => await _movieRepository.GetByYearAsync(year, pageNumber, pageSize);

    public async Task<Movie> GetMovieAsync(string id)
    {
        var movie = await _movieRepository.GetAsync(id);
        if (movie is null)
            throw new EntityNotFoundException($"movie not found: {id}");

        return movie;
    }

    public async Task<IEnumerable<Movie>> GetNomineesAsync(int year) =>
        await _movieRepository.GetNomineesByYearAsync(year);

    public async Task<Movie> AddMovieAsync(
        string name,
        int year,
        Month month,
        Cinema cinema,
        int releaseYear,
        string? zinger,
        string? alias,
        string? motif,
        bool isNominated,
        bool isKino,
        bool isRetro
    )
    {
        if (isKino && !isNominated)
            throw new DomainValidationException(
                "movie cannot be KINO without also being nominated"
            );
        if (isKino && await _movieRepository.CheckKinoExistenceAsync(year))
            throw new DomainValidationException($"{year} has already KINO'd");

        var movie = await _movieRepository.AddAsync(
            new Movie
            {
                Name = name,
                Year = year,
                Month = month,
                Cinema = cinema,
                ReleaseYear = releaseYear,
                Zinger = zinger,
                Alias = alias,
                Motif = motif,
                IsNominated = isNominated,
                IsKino = isKino,
                IsRetro = isRetro
            }
        );

        return movie;
    }

    public async Task<Movie> UpdateMovieAsync(
        string id,
        string name,
        int year,
        Month month,
        Cinema cinema,
        int releaseYear,
        string? zinger,
        string? alias,
        string? motif,
        string? imageUrl,
        string? imageObjectKey,
        string? altImageUrl,
        string? altImageObjectKey,
        bool isNominated,
        bool isKino,
        bool isRetro
    )
    {
        if (isKino && !isNominated)
            throw new DomainValidationException(
                "movie cannot be KINO without also being nominated"
            );
        if (isKino && await _movieRepository.CheckKinoExistenceAsync(year, id))
            throw new DomainValidationException($"{year} has already KINO'd");

        var updatedMovie = new Movie
        {
            Id = id,
            Name = name,
            Year = year,
            Month = month,
            Cinema = cinema,
            ReleaseYear = releaseYear,
            Zinger = zinger,
            Alias = alias,
            Motif = motif,
            ImageUrl = imageUrl,
            ImageObjectKey = imageObjectKey,
            AltImageUrl = altImageUrl,
            AltImageObjectKey = altImageObjectKey,
            IsNominated = isNominated,
            IsKino = isKino,
            IsRetro = isRetro
        };
        var result = await _movieRepository.UpdateAsync(id, updatedMovie);
        if (!result)
            throw new EntityNotFoundException($"movie not found: {id}");

        return updatedMovie;
    }

    public async Task<long> UpdateCinemaOfMoviesAsync(string cinemaId, Cinema cinema) =>
        await _movieRepository.UpdateCinemaInfoAsync(cinemaId, cinema);

    public async Task RemoveMovieAsync(string id)
    {
        var movie = await _movieRepository.GetAsync(id);
        if (movie is null)
            throw new EntityNotFoundException($"movie not found: {id}");

        if (!string.IsNullOrEmpty(movie.ImageObjectKey))
            await _imageStorage.DeleteObjectAsync(movie.ImageObjectKey);
        if (!string.IsNullOrEmpty(movie.AltImageObjectKey))
            await _imageStorage.DeleteObjectAsync(movie.AltImageObjectKey);

        await _movieRepository.RemoveAsync(id);
    }

    public async Task<bool> CheckIfAnyMoviesAssociatedWithCinemaAsync(string cinemaId) =>
        await _movieRepository.CheckCinemaAssociationExistenceAsync(cinemaId);

    public async Task<IEnumerable<int>> GetActiveYearsAsync() =>
        await _movieRepository.GetActiveYearsAsync();

    public async Task<string> HandleImageUploadAsync(
        string id,
        string imageBasePath,
        string fileExtension,
        bool isAlt
    )
    {
        var movie = await _movieRepository.GetAsync(id);
        if (movie is null)
            throw new EntityNotFoundException($"movie not found: {id}");

        if (isAlt && !movie.IsNominated)
            throw new DomainValidationException($"movies cannot have alt images if not nominated");

        if (isAlt)
        {
            if (!string.IsNullOrEmpty(movie.AltImageObjectKey))
                await _imageStorage.DeleteObjectAsync(movie.AltImageObjectKey);
        }
        else
        {
            if (!string.IsNullOrEmpty(movie.ImageObjectKey))
                await _imageStorage.DeleteObjectAsync(movie.ImageObjectKey);
        }

        var objectKey = isAlt
            ? $"{imageBasePath}/{movie.Year}/{id}-alt.{fileExtension}"
            : $"{imageBasePath}/{movie.Year}/{id}.{fileExtension}";
        var presignedUrl = await _imageStorage.GeneratePresignedUploadUrlAsync(
            objectKey,
            TimeSpan.FromMinutes(5)
        );
        var publicUrl = _imageStorage.GetPublicUrl(objectKey);

        await _movieRepository.UpdateImageInfoAsync(id, objectKey, publicUrl, isAlt);

        return presignedUrl;
    }
}

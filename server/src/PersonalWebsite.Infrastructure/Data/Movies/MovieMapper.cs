using PersonalWebsite.Core.Models;
using PersonalWebsite.Infrastructure.Data.Cinemas;

namespace PersonalWebsite.Infrastructure.Data.Movies;

public static class MovieMapper
{
    public static Movie ToDomain(MovieDocument document) =>
        new Movie
        {
            Id = document.Id,
            Name = document.Name,
            Year = document.Year,
            Month = document.Month,
            Cinema = CinemaMapper.ToDomain(document.Cinema),
            ReleaseYear = document.ReleaseYear,
            IsNominated = document.IsNominated,
            IsKino = document.IsKino,
            IsRetro = document.IsRetro,
            Zinger = document.Zinger,
            Alias = document.Alias,
            Motif = document.Motif,
            ImageUrl = document.ImageUrl,
            ImageObjectKey = document.ImageObjectKey,
            AltImageUrl = document.AltImageUrl,
            AltImageObjectKey = document.AltImageObjectKey,
        };

    public static MovieDocument ToDocument(Movie movie) =>
        new MovieDocument
        {
            Id = movie.Id,
            Name = movie.Name,
            Year = movie.Year,
            Month = movie.Month,
            Cinema = CinemaMapper.ToDocument(movie.Cinema),
            ReleaseYear = movie.ReleaseYear,
            IsNominated = movie.IsNominated,
            IsRetro = movie.IsRetro,
            IsKino = movie.IsKino,
            Zinger = movie.Zinger,
            Alias = movie.Alias,
            Motif = movie.Motif,
            ImageUrl = movie.ImageUrl,
            ImageObjectKey = movie.ImageObjectKey,
            AltImageUrl = movie.AltImageUrl,
            AltImageObjectKey = movie.AltImageObjectKey,
        };
}

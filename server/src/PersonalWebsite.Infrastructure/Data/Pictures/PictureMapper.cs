using PersonalWebsite.Core.Models;
using PersonalWebsite.Infrastructure.Data.Cinemas;

namespace PersonalWebsite.Infrastructure.Data.Pictures;

public static class PictureMapper
{
    public static Picture ToDomain(PictureDocument document) =>
        new Picture
        {
            Id = document.Id,
            Name = document.Name,
            YearWatched = document.YearWatched,
            MonthWatched = document.MonthWatched,
            Cinema = CinemaMapper.ToDomain(document.Cinema),
            YearReleased = document.YearReleased,
            Zinger = document.Zinger,
            IsFavorite = document.IsFavorite,
            IsKino = document.IsKino,
            IsNewRelease = document.IsNewRelease,
            Alias = document.Alias,
            ImageUrl = document.ImageUrl,
            AltImageUrl = document.AltImageUrl,
        };

    public static PictureDocument ToDocument(Picture picture) =>
        new PictureDocument
        {
            Id = picture.Id,
            Name = picture.Name,
            YearWatched = picture.YearWatched,
            MonthWatched = picture.MonthWatched,
            Cinema = CinemaMapper.ToDocument(picture.Cinema),
            YearReleased = picture.YearReleased,
            Zinger = picture.Zinger,
            IsFavorite = picture.IsFavorite,
            IsNewRelease = picture.IsNewRelease,
            IsKino = picture.IsKino,
            Alias = picture.Alias,
            ImageUrl = picture.ImageUrl,
            AltImageUrl = picture.AltImageUrl,
        };
}

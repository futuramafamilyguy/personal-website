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
            Zinger = document.Zinger,
            IsFavorite = document.IsFavorite,
            Alias = document.Alias,
            ImageUrl = document.ImageUrl,
        };

    public static PictureDocument ToDocument(Picture picture) =>
        new PictureDocument
        {
            Id = picture.Id,
            Name = picture.Name,
            YearWatched = picture.YearWatched,
            MonthWatched = picture.MonthWatched,
            Cinema = CinemaMapper.ToDocument(picture.Cinema),
            Zinger = picture.Zinger,
            IsFavorite = picture.IsFavorite,
            Alias = picture.Alias,
            ImageUrl = picture.ImageUrl,
        };
}

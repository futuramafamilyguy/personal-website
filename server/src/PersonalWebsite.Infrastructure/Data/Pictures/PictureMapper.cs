using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Infrastructure.Data.Pictures;

public static class PictureMapper
{
    public static Picture ToDomain(PictureDocument document)
        => new Picture
        {
            Id = document.Id,
            Name = document.Name,
            Year = document.Year,
            Zinger = document.Zinger
        };

    public static PictureDocument ToDocument(Picture picture)
        => new PictureDocument
        {
            Id = picture.Id,
            Name = picture.Name,
            Year = picture.Year,
            Zinger = picture.Zinger
        };
}

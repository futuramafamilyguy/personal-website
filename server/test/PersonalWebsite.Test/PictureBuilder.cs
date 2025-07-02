using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Test;

public class PictureBuilder
{
    private Picture _picture = new Picture
    {
        Name = "Cars",
        YearWatched = 2020,
        MonthWatched = Core.Enums.Month.Jan,
        Cinema = new Cinema { City = "Christchuch", Name = "Alice" },
        YearReleased = 2006,
        IsFavorite = false,
        IsKino = false,
        IsNewRelease = false
    };

    public PictureBuilder(string id)
    {
        _picture.Id = id;
    }

    public PictureBuilder WithName(string name)
    {
        _picture.Name = name;
        return this;
    }

    public PictureBuilder WithYearWatched(int year)
    {
        _picture.YearWatched = year;
        return this;
    }

    public PictureBuilder WithImageUrl(string url)
    {
        _picture.ImageUrl = url;
        return this;
    }

    public PictureBuilder WithFavorite(bool isFavorite)
    {
        _picture.IsFavorite = isFavorite;
        return this;
    }

    public PictureBuilder WithKino(bool isKino)
    {
        _picture.IsKino = isKino;
        return this;
    }

    public PictureBuilder WithImage(string imageStorageHost, string objectKey)
    {
        _picture.ImageUrl = $"{imageStorageHost}/{objectKey}";
        _picture.ImageObjectKey = objectKey;
        return this;
    }

    public PictureBuilder WithAltImage(string imageStorageHost, string objectKey)
    {
        _picture.AltImageUrl = $"{imageStorageHost}/{objectKey}";
        _picture.AltImageObjectKey = objectKey;
        return this;
    }

    public Picture Build() => _picture;
}

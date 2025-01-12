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

    public Picture Build() => _picture;
}

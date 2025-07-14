using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Test;

public class MovieBuilder
{
    private Movie _movie = new Movie
    {
        Name = "Cars",
        Year = 2020,
        Month = Core.Enums.Month.Jan,
        Cinema = new Cinema { City = "Christchuch", Name = "Alice" },
        ReleaseYear = 2006,
        IsNominated = false,
        IsKino = false,
        IsRetro = false
    };

    public MovieBuilder(string id)
    {
        _movie.Id = id;
    }

    public MovieBuilder WithName(string name)
    {
        _movie.Name = name;
        return this;
    }

    public MovieBuilder WithYearWatched(int year)
    {
        _movie.Year = year;
        return this;
    }

    public MovieBuilder WithImageUrl(string url)
    {
        _movie.ImageUrl = url;
        return this;
    }

    public MovieBuilder WithFavorite(bool isFavorite)
    {
        _movie.IsNominated = isFavorite;
        return this;
    }

    public MovieBuilder WithKino(bool isKino)
    {
        _movie.IsKino = isKino;
        return this;
    }

    public MovieBuilder WithImage(string imageStorageHost, string objectKey)
    {
        _movie.ImageUrl = $"{imageStorageHost}/{objectKey}";
        _movie.ImageObjectKey = objectKey;
        return this;
    }

    public MovieBuilder WithAltImage(string imageStorageHost, string objectKey)
    {
        _movie.AltImageUrl = $"{imageStorageHost}/{objectKey}";
        _movie.AltImageObjectKey = objectKey;
        return this;
    }

    public Movie Build() => _movie;
}

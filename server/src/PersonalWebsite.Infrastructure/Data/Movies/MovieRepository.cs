using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;
using PersonalWebsite.Infrastructure.Data.Cinemas;
using PersonalWebsite.Infrastructure.Data.Movies;

namespace PersonalWebsite.Infrastructure.Data.Movies;

public class MovieRepository : IMovieRepository
{
    private readonly IMongoCollection<MovieDocument> _movies;
    private readonly MongoDbConfiguration _settings;

    public MovieRepository(IMongoClient client, IOptions<MongoDbConfiguration> configuration)
    {
        _settings = configuration.Value;
        var database = client.GetDatabase(_settings.DatabaseName);
        _movies = database.GetCollection<MovieDocument>(_settings.MoviesCollectionName);
    }

    public async Task<Movie?> GetAsync(string id)
    {
        var filter = Builders<MovieDocument>.Filter.Eq(movie => movie.Id, id);
        var movie = await _movies.Find(filter).FirstOrDefaultAsync();
        if (movie is null)
            return null;

        return MovieMapper.ToDomain(movie);
    }

    public async Task<(IEnumerable<Movie> Movies, long TotalCount)> GetByYearAsync(
        int year,
        int pageNumber,
        int pageSize
    )
    {
        var filter = Builders<MovieDocument>.Filter.Eq(movie => movie.Year, year);
        var sort = SortMoviesByWatchDate();
        var totalCount = await _movies.CountDocumentsAsync(filter);
        var movies = await _movies
            .Find(filter)
            .Sort(sort)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (movies.Select(MovieMapper.ToDomain), totalCount);
    }

    public async Task<IEnumerable<Movie>> GetNomineesByYearAsync(int year)
    {
        var sort = SortMoviesByWatchDate();
        var filter = Builders<MovieDocument>.Filter.Where(movie =>
            movie.Year == year && movie.IsNominated
        );
        var movies = await _movies.Find(filter).Sort(sort).ToListAsync();

        return movies.Select(MovieMapper.ToDomain);
    }

    public async Task<Movie> AddAsync(Movie movie)
    {
        var movieDocument = MovieMapper.ToDocument(movie);
        await _movies.InsertOneAsync(movieDocument);
        movie.Id = movieDocument.Id;

        return movie;
    }

    public async Task RemoveAsync(string id) =>
        await _movies.DeleteOneAsync(movie => movie.Id == id);

    public async Task<bool> UpdateAsync(string id, Movie updatedMovie)
    {
        var result = await _movies.ReplaceOneAsync(
            movie => movie.Id == id,
            MovieMapper.ToDocument(updatedMovie)
        );

        return result.MatchedCount == 1;
    }

    public async Task<long> UpdateCinemaInfoAsync(string cinemaId, Cinema updatedCinema)
    {
        var filter = Builders<MovieDocument>.Filter.Eq(movie => movie.Cinema.Id, cinemaId);
        var updateQuery = Builders<MovieDocument>.Update.Set(
            movie => movie.Cinema,
            CinemaMapper.ToDocument(updatedCinema)
        );
        var result = await _movies.UpdateManyAsync(filter, updateQuery);

        return result.MatchedCount;
    }

    public async Task<bool> CheckCinemaAssociationExistenceAsync(string cinemaId)
    {
        var filter = Builders<MovieDocument>.Filter.Eq(movie => movie.Cinema.Id, cinemaId);

        return await _movies.Find(filter).AnyAsync();
    }

    public async Task<IEnumerable<int>> GetActiveYearsAsync()
    {
        var activeYears = await _movies
            .Aggregate()
            .Group(movie => movie.Year, group => new { Year = group.Key })
            .Project(group => group.Year)
            .ToListAsync();

        return activeYears;
    }

    public async Task<bool> CheckKinoExistenceAsync(int year, string? id = null)
    {
        var filter = Builders<MovieDocument>.Filter.And(
            Builders<MovieDocument>.Filter.Eq(movie => movie.Year, year),
            Builders<MovieDocument>.Filter.Eq(movie => movie.IsKino, true)
        );

        if (id is not null)
        {
            filter = Builders<MovieDocument>.Filter.And(
                filter,
                Builders<MovieDocument>.Filter.Ne(movie => movie.Id, id)
            );
        }

        return await _movies.Find(filter).AnyAsync();
    }

    public async Task UpdateImageInfoAsync(
        string id,
        string imageObjectKey,
        string imageUrl,
        bool isAlt
    )
    {
        var filter = Builders<MovieDocument>.Filter.Eq(post => post.Id, id);

        if (isAlt)
        {
            var updateAlt = Builders<MovieDocument>
                .Update.Set(movie => movie.AltImageObjectKey, imageObjectKey)
                .Set(movie => movie.AltImageUrl, imageUrl);

            await _movies.UpdateOneAsync(filter, updateAlt);

            return;
        }

        var update = Builders<MovieDocument>
            .Update.Set(movie => movie.ImageObjectKey, imageObjectKey)
            .Set(movie => movie.ImageUrl, imageUrl);

        await _movies.UpdateOneAsync(filter, update);
    }

    private static SortDefinition<MovieDocument> SortMoviesByWatchDate() =>
        Builders<MovieDocument>
            .Sort.Descending(movie => movie.Month)
            .Descending(movie => movie.Id);
}

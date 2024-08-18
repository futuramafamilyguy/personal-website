using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;
using PersonalWebsite.Infrastructure.Data.Cinemas;

namespace PersonalWebsite.Infrastructure.Data.Pictures;

public class PictureRepository : IPictureRepository
{
    private readonly IMongoCollection<PictureDocument> _pictures;
    private readonly MongoDbConfiguration _configuration;

    public PictureRepository(IMongoClient client, IOptions<MongoDbConfiguration> settings)
    {
        _configuration = settings.Value;
        var database = client.GetDatabase(_configuration.DatabaseName);
        _pictures = database.GetCollection<PictureDocument>(_configuration.PicturesCollectionName);
    }

    public async Task<Picture> GetAsync(string id)
    {
        var filter = Builders<PictureDocument>.Filter.Eq(picture => picture.Id, id);
        var picture = await _pictures.Find(filter).FirstOrDefaultAsync();

        return PictureMapper.ToDomain(picture);
    }

    public async Task<(IEnumerable<Picture> Pictures, long TotalCount)> GetByYearWatchedAsync(
        int yearWatched,
        int pageNumber,
        int pageSize
    )
    {
        var filter = Builders<PictureDocument>.Filter.Eq(
            picture => picture.YearWatched,
            yearWatched
        );
        var sort = SortPicturesByWatchDate();
        var totalCount = await _pictures.CountDocumentsAsync(filter);
        var pictures = await _pictures
            .Find(filter)
            .Sort(sort)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (pictures.Select(PictureMapper.ToDomain), totalCount);
    }

    public async Task<IEnumerable<Picture>> GetFavoritesByYearWatchedAsync(int yearWatched)
    {
        var sort = SortPicturesByWatchDate();
        var filter = Builders<PictureDocument>.Filter.Where(picture =>
            picture.YearWatched == yearWatched && picture.IsFavorite
        );
        var pictures = await _pictures.Find(filter).Sort(sort).ToListAsync();

        return pictures.Select(PictureMapper.ToDomain);
    }

    public async Task<Picture> AddAsync(Picture picture)
    {
        var pictureDocument = PictureMapper.ToDocument(picture);
        await _pictures.InsertOneAsync(pictureDocument);
        picture.Id = pictureDocument.Id;

        return picture;
    }

    public async Task RemoveAsync(string id) =>
        await _pictures.DeleteOneAsync(picture => picture.Id == id);

    public async Task<Picture> UpdateAsync(string id, Picture updatedPicture)
    {
        await _pictures.ReplaceOneAsync(
            picture => picture.Id == id,
            PictureMapper.ToDocument(updatedPicture)
        );

        return updatedPicture;
    }

    public async Task<long> UpdateCinemaInfoAsync(string cinemaId, Cinema updatedCinema)
    {
        var filter = Builders<PictureDocument>.Filter.Eq(picture => picture.Cinema.Id, cinemaId);
        var updateQuery = Builders<PictureDocument>.Update.Set(
            picture => picture.Cinema,
            CinemaMapper.ToDocument(updatedCinema)
        );
        var result = await _pictures.UpdateManyAsync(filter, updateQuery);

        return result.MatchedCount;
    }

    public async Task<bool> CheckCinemaAssociationExistenceAsync(string cinemaId)
    {
        var filter = Builders<PictureDocument>.Filter.Eq(picture => picture.Cinema.Id, cinemaId);

        return await _pictures.Find(filter).AnyAsync();
    }

    public async Task<IEnumerable<int>> GetActiveYearsAsync()
    {
        var activeYears = await _pictures
            .Aggregate()
            .Group(picture => picture.YearWatched, group => new { Year = group.Key })
            .Project(group => group.Year)
            .ToListAsync();

        return activeYears;
    }

    private static SortDefinition<PictureDocument> SortPicturesByWatchDate() =>
        Builders<PictureDocument>
            .Sort.Descending(picture => picture.MonthWatched)
            .Descending(picture => picture.Id);
}

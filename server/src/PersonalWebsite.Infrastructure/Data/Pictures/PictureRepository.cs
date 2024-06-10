using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;
using PersonalWebsite.Infrastructure.Data.Cinemas;

namespace PersonalWebsite.Infrastructure.Data.Pictures;

public class PictureRepository : IPictureRepository
{
    private readonly IMongoCollection<PictureDocument> _pictures;
    private readonly MongoDbConfiguration _settings;

    public PictureRepository(IMongoClient client, IOptions<MongoDbConfiguration> settings)
    {
        _settings = settings.Value;
        var database = client.GetDatabase(_settings.DatabaseName);
        _pictures = database.GetCollection<PictureDocument>(_settings.PicturesCollectionName);
    }

    public async Task<IEnumerable<Picture>> GetByYearAsync(int year)
    {
        var filter = Builders<PictureDocument>.Filter.Eq(picture => picture.Year, year);
        var pictures = await _pictures.Find(filter).ToListAsync();
        
        return pictures.Select(PictureMapper.ToDomain);
    }

    public async Task<IEnumerable<Picture>> GetFavoriteByYearAsync(int year)
    {
        var filter = Builders<PictureDocument>.Filter.Where(picture => picture.Year == year && picture.IsFavorite);
        var pictures = await _pictures.Find(filter).ToListAsync();

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
        await _pictures.ReplaceOneAsync(picture => picture.Id == id, PictureMapper.ToDocument(updatedPicture));

        return updatedPicture;
    }

    public async Task<Picture> ToggleFavoriteStatusAsync(string id)
    {
        var filter = Builders<PictureDocument>.Filter.Eq(picture => picture.Id, id);
        var updatedPicture = await _pictures.Find(filter).FirstOrDefaultAsync();
        updatedPicture.IsFavorite = !updatedPicture.IsFavorite;
        var updateQuery = Builders<PictureDocument>.Update.Set(picture => picture.IsFavorite, updatedPicture.IsFavorite);
        await _pictures.UpdateOneAsync(filter, updateQuery);

        return PictureMapper.ToDomain(updatedPicture);
    }

    public async Task UpdateCinemaInfoAsync(string cinemaId, Cinema updatedCinema)
    {
        var filter = Builders<PictureDocument>.Filter.Eq(picture => picture.Cinema.Id, cinemaId);
        var updateQuery = Builders<PictureDocument>.Update.Set(picture => picture.Cinema, CinemaMapper.ToDocument(updatedCinema));
        await _pictures.UpdateManyAsync(filter, updateQuery);
    }

    public async Task<bool> CheckCinemaAssociationExistenceAsync(string cinemaId)
    {
        var filter = Builders<PictureDocument>.Filter.Eq(picture => picture.Cinema.Id, cinemaId);

        return await _pictures.Find(filter).AnyAsync();
    }
}

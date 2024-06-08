using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

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
        
        return pictures.Select(picture => PictureMapper.ToDomain(picture));
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
}

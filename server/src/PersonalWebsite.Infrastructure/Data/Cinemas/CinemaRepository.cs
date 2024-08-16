using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Infrastructure.Data.Cinemas;

public class CinemaRepository : ICinemaRepository
{
    private readonly IMongoCollection<CinemaDocument> _cinemas;
    private readonly MongoDbConfiguration _settings;

    public CinemaRepository(IMongoClient client, IOptions<MongoDbConfiguration> settings)
    {
        _settings = settings.Value;
        var database = client.GetDatabase(_settings.DatabaseName);
        _cinemas = database.GetCollection<CinemaDocument>(_settings.CinemasCollectionName);
    }

    public async Task<Cinema> AddAsync(Cinema cinema)
    {
        var cinemaDocument = CinemaMapper.ToDocument(cinema);
        await _cinemas.InsertOneAsync(cinemaDocument);
        cinema.Id = cinemaDocument.Id;

        return cinema;
    }

    public async Task<IEnumerable<Cinema>> GetAsync()
    {
        var sort = Builders<CinemaDocument>.Sort.Ascending(cinema => cinema.Name);
        var cinemas = await _cinemas
            .Find(FilterDefinition<CinemaDocument>.Empty)
            .Sort(sort)
            .ToListAsync();

        return cinemas.Select(CinemaMapper.ToDomain);
    }

    public async Task<Cinema> GetAsync(string id)
    {
        var filter = Builders<CinemaDocument>.Filter.Eq(cinema => cinema.Id, id);
        var cinema = await _cinemas.Find(filter).FirstOrDefaultAsync();

        return CinemaMapper.ToDomain(cinema);
    }

    public async Task RemoveAsync(string id) =>
        await _cinemas.DeleteOneAsync(cinema => cinema.Id == id);

    public async Task<Cinema> UpdateAsync(string id, Cinema updatedCinema)
    {
        await _cinemas.ReplaceOneAsync(
            cinema => cinema.Id == id,
            CinemaMapper.ToDocument(updatedCinema)
        );

        return updatedCinema;
    }
}

using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class CinemaService : ICinemaService
{
    private readonly ICinemaRepository _cinemaRepository;

    public CinemaService(ICinemaRepository cinemaRepository)
    {
        _cinemaRepository = cinemaRepository;
    }

    public async Task<Cinema> AddCinemaAsync(string name, string city)
    {
        var cinema = await _cinemaRepository.AddAsync(new Cinema
        {
            Name = name,
            City = city
        });

        return cinema;
    }

    public async Task<IEnumerable<Cinema>> GetCinemasAsync()
    {
        var cinemas = await _cinemaRepository.GetAsync();

        return cinemas;
    }

    public async Task<Cinema> GetCinemaAsync(string id)
    {
        var cinema = await _cinemaRepository.GetAsync(id);

        return cinema;
    }

    public async Task RemoveCinemaAsync(string id) =>
        await _cinemaRepository.RemoveAsync(id);

    public async Task<Cinema> UpdateCinemaAsync(string id, string name, string city)
    {
        var updatedCinema = await _cinemaRepository.UpdateAsync(id, new Cinema
        {
            Id = id,
            Name = name,
            City = city
        });

        return updatedCinema;
    }
}

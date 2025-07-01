using PersonalWebsite.Core.Exceptions;
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
        var cinema = await _cinemaRepository.AddAsync(new Cinema { Name = name, City = city });

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
        if (cinema is null)
            throw new EntityNotFoundException($"cinema not found: {id}");

        return cinema;
    }

    public async Task RemoveCinemaAsync(string id)
    {
        var result = await _cinemaRepository.RemoveAsync(id);
        if (!result)
            throw new EntityNotFoundException($"cinema not found: {id}");
    }

    public async Task<Cinema> UpdateCinemaAsync(string id, string name, string city)
    {
        var updatedCinema = new Cinema
        {
            Id = id,
            Name = name,
            City = city
        };
        var result = await _cinemaRepository.UpdateAsync(id, updatedCinema);
        if (!result)
            throw new EntityNotFoundException($"cinema not found: {id}");

        return updatedCinema;
    }
}

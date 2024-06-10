using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface ICinemaRepository
{
    Task<IEnumerable<Cinema>> GetAsync();
    Task<Cinema> GetAsync(string id);
    Task<Cinema> AddAsync(Cinema cinema);
    Task<Cinema> UpdateAsync(string id, Cinema updatedCinema);
    Task RemoveAsync(string id);
}

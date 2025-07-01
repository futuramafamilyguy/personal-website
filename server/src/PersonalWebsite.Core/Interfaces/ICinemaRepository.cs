using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface ICinemaRepository
{
    Task<IEnumerable<Cinema>> GetAsync();
    Task<Cinema?> GetAsync(string id);
    Task<Cinema> AddAsync(Cinema cinema);
    Task<bool> UpdateAsync(string id, Cinema updatedCinema);
    Task<bool> RemoveAsync(string id);
}

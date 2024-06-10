using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface ICinemaService
{
    Task<IEnumerable<Cinema>> GetCinemasAsync();
    Task<Cinema> GetCinemaAsync(string id);
    Task<Cinema> AddCinemaAsync(string name, string city);
    Task<Cinema> UpdateCinemaAsync(string id, string name, string city);
    Task RemoveCinemaAsync(string id);
}

using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPictureRepository
{
    Task<IEnumerable<Picture>> GetByYearAsync(int year);
    Task<IEnumerable<Picture>> GetFavoriteByYearAsync(int year);
    Task<Picture> AddAsync(Picture picture);
    Task<Picture> UpdateAsync(string id, Picture picture);
    Task<Picture> ToggleFavoriteStatusAsync(string id);
    Task RemoveAsync(string id);
}

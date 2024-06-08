using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPictureRepository
{
    Task<IEnumerable<Picture>> GetByYearAsync(int year);
    Task<Picture> AddAsync(Picture picture);
    Task<Picture> UpdateAsync(string id, Picture picture);
    Task RemoveAsync(string id);
}

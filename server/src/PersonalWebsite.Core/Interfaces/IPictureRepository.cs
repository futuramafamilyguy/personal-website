using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPictureRepository
{
    Task<IEnumerable<Picture>> GetByYearAsync(int year);
    Task AddAsync(Picture picture);
    Task UpdateAsync(string id, Picture picture);
    Task RemoveAsync(string id);
}

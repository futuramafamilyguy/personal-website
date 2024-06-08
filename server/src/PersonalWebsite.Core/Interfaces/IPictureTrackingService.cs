using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPictureTrackingService
{
    Task<IEnumerable<Picture>> GetPicturesAsync(int year);
    Task<Picture> AddPictureAsync(string name, int year, string? zinger);
    Task<Picture> UpdatePictureAsync(string id, string name, int year, string? zinger);
    Task RemovePictureAsync(string id);
}

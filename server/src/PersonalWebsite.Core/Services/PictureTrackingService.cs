﻿using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class PictureTrackingService : IPictureTrackingService
{
    private readonly IPictureRepository _pictureRepository;

    public PictureTrackingService(IPictureRepository pictureRepository)
    {
        _pictureRepository = pictureRepository;
    }

    public async Task<IEnumerable<Picture>> GetPicturesAsync(int year)
        => await _pictureRepository.GetByYearAsync(year);

    public async Task<IEnumerable<Picture>> GetFavoritePicturesAsync(int year)
        => await _pictureRepository.GetFavoriteByYearAsync(year);

    public async Task<Picture> AddPictureAsync(string name, int year, string? zinger)
    {
        var picture = await _pictureRepository.AddAsync(new Picture
        {
            Name = name,
            Year = year,
            Zinger = zinger
        });

        return picture;
    }

    public async Task<Picture> UpdatePictureAsync(string id, string name, int year, string? zinger)
    {
        var updatedPicture = await _pictureRepository.UpdateAsync(id, new Picture
        {
            Id = id,
            Name = name,
            Year = year,
            Zinger = zinger
        });

        return updatedPicture;
    }

    public async Task<Picture> ToggleFavoriteAsync(string id)
    {
        var updatedPicture = await _pictureRepository.ToggleFavoriteStatusAsync(id);

        return updatedPicture;
    }

    public async Task RemovePictureAsync(string id)
    {
        await _pictureRepository.RemoveAsync(id);
    }
}

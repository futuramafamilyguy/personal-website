using Moq;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;
using PersonalWebsite.Core.Services;

namespace PersonalWebsite.Test;

public class PictureTrackingServiceTest
{
    [Fact]
    public async Task GetPicturesAsync_ShouldCallGetByYearAsync()
    {
        var repository = new Mock<IPictureRepository>();
        var yearWatched = 2020;
        var sut = new PictureTrackingService(repository.Object);

        await sut.GetPicturesAsync(yearWatched);

        repository.Verify(
            x => x.GetByYearAsync(yearWatched),
            Times.Once());
    }

    [Fact]
    public async Task GetFavoritePicturesAsync_ShouldCallGetFavoriteByYearAsync()
    {
        var repository = new Mock<IPictureRepository>();
        var yearWatched = 2020;
        var sut = new PictureTrackingService(repository.Object);

        await sut.GetFavoritePicturesAsync(yearWatched);

        repository.Verify(
            x => x.GetFavoriteByYearAsync(yearWatched),
            Times.Once());
    }

    [Theory]
    [InlineData("bazinga")]
    [InlineData(null)]
    public async Task AddPictureAsync_ShouldCallAddAsync(string zinger)
    {
        var repository = new Mock<IPictureRepository>();
        var pictureName = "cars";
        var yearWatched = 2020;
        var sut = new PictureTrackingService(repository.Object);

        await sut.AddPictureAsync(pictureName, yearWatched, zinger);

        repository.Verify(
            x => x.AddAsync(
                It.Is((Picture picture) => picture.Name == pictureName
                    && picture.Year == yearWatched
                    && picture.Zinger == zinger)),
                Times.Once());
    }

    [Theory]
    [InlineData("bazinga")]
    [InlineData(null)]
    public async Task UpdatePictureAsync_ShouldCallUpdateAsync(string updatedZinger)
    {
        var repository = new Mock<IPictureRepository>();
        var pictureId = "123";
        var updatedPictureName = "cars";
        var updatedYearWatched = 2020;
        var sut = new PictureTrackingService(repository.Object);

        await sut.UpdatePictureAsync(pictureId, updatedPictureName, updatedYearWatched, updatedZinger);

        repository.Verify(
            x => x.UpdateAsync(
                pictureId,
                It.Is((Picture picture) => picture.Name == updatedPictureName
                    && picture.Year == updatedYearWatched
                    && picture.Zinger == updatedZinger)),
                Times.Once());
    }

    [Fact]
    public async Task RemovePictureAsync_ShouldCallRemoveAsync()
    {
        var repository = new Mock<IPictureRepository>();
        var pictureId = "123";
        var sut = new PictureTrackingService(repository.Object);

        await sut.RemovePictureAsync(pictureId);

        repository.Verify(
            x => x.RemoveAsync(pictureId),
            Times.Once());
    }

    [Fact]
    public async Task ToggleFavoriteAsync_ShouldCallToggleFavoriteStatusAsync()
    {
        var repository = new Mock<IPictureRepository>();
        var pictureId = "123";
        var sut = new PictureTrackingService(repository.Object);

        await sut.ToggleFavoriteAsync(pictureId);

        repository.Verify(
            x => x.ToggleFavoriteStatusAsync(pictureId),
            Times.Once());
    }
}

using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;
using PersonalWebsite.Core.Services;

namespace PersonalWebsite.Test;

public class PictureServiceTests
{
    [Fact]
    public async Task GetPicturesAsync_ShouldCallGetByYearAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        var yearWatched = 2020;

        var pageNumber = 1;
        var pageSize = int.MaxValue;

        // act
        await sut.GetPicturesAsync(yearWatched, pageNumber, pageSize);

        // assert
        repositoryMock.Verify(
            x => x.GetByYearWatchedAsync(yearWatched, pageNumber, pageSize),
            Times.Once()
        );
    }

    [Fact]
    public async Task GetPictureAsync_ShouldCallGetAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        var id = "123";

        // act
        await sut.GetPictureAsync(id);

        // assert
        repositoryMock.Verify(x => x.GetAsync(id), Times.Once());
    }

    [Fact]
    public async Task GetFavoritePicturesAsync_ShouldCallGetFavoriteByYearAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        var yearWatched = 2020;

        // act
        await sut.GetFavoritePicturesAsync(yearWatched);

        // assert
        repositoryMock.Verify(x => x.GetFavoritesByYearWatchedAsync(yearWatched), Times.Once());
    }

    [Fact]
    public async Task AddPictureAsync_ShouldCallAddAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        var pictureName = "cars";
        var yearWatched = 2020;
        var monthWatched = Month.Jan;
        var yearReleased = 2020;
        var zinger = "bazinga";
        var alias = "car";
        var isFavorite = false;

        var cinemaId = "123";
        var cinemaName = "Alice";
        var city = "Christchurch";
        var cinema = new Cinema
        {
            Id = cinemaId,
            Name = cinemaName,
            City = city
        };

        // act
        await sut.AddPictureAsync(
            pictureName,
            yearWatched,
            monthWatched,
            cinema,
            yearReleased,
            zinger,
            alias,
            isFavorite
        );

        // assert
        repositoryMock.Verify(
            x =>
                x.AddAsync(
                    It.Is(
                        (Picture picture) =>
                            picture.Name == pictureName
                            && picture.YearWatched == yearWatched
                            && picture.MonthWatched == monthWatched
                            && picture.Zinger == zinger
                            && picture.Cinema.Id == cinemaId
                            && picture.Cinema.Name == cinemaName
                            && picture.Cinema.City == city
                            && picture.YearReleased == yearReleased
                            && picture.Alias == alias
                            && picture.IsFavorite == isFavorite
                    )
                ),
            Times.Once()
        );
    }

    [Fact]
    public async Task UpdatePictureAsync_ShouldCallUpdateAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        var pictureId = "123";
        var updatedPictureName = "cars";
        var updatedYearWatched = 2020;
        var updatedMonthWatched = Month.Jan;
        var updatedYearReleased = 2020;
        var updatedZinger = "bazinga";
        var updatedAlias = "car";
        var updatedImageUrl = "domain/images/image.jpg";
        var updatedIsFavorite = true;

        var cinemaId = "123";
        var cinemaName = "Alice";
        var city = "Christchurch";
        var updatedCinema = new Cinema
        {
            Id = cinemaId,
            Name = cinemaName,
            City = city
        };

        // act
        await sut.UpdatePictureAsync(
            pictureId,
            updatedPictureName,
            updatedYearWatched,
            updatedMonthWatched,
            updatedCinema,
            updatedYearReleased,
            updatedZinger,
            updatedAlias,
            updatedImageUrl,
            updatedIsFavorite
        );

        // assert
        repositoryMock.Verify(
            x =>
                x.UpdateAsync(
                    pictureId,
                    It.Is(
                        (Picture picture) =>
                            picture.Name == updatedPictureName
                            && picture.YearWatched == updatedYearWatched
                            && picture.MonthWatched == updatedMonthWatched
                            && picture.Zinger == updatedZinger
                            && picture.Cinema.Id == cinemaId
                            && picture.Cinema.Name == cinemaName
                            && picture.Cinema.City == city
                            && picture.YearReleased == updatedYearReleased
                            && picture.Alias == updatedAlias
                            && picture.IsFavorite == updatedIsFavorite
                    )
                ),
            Times.Once()
        );
    }

    [Fact]
    public async Task RemovePictureAsync_ShouldCallRemoveAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        var pictureId = "123";

        // act
        await sut.RemovePictureAsync(pictureId);

        // assert
        repositoryMock.Verify(x => x.RemoveAsync(pictureId), Times.Once());
    }

    [Fact]
    public async Task UpdateCinemaOfPicturesAsync_ShouldCallUpdateCinemaInfoAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        var cinemaId = "123";
        var updatedCinema = new Cinema
        {
            Id = cinemaId,
            Name = "Alice",
            City = "Christchurch"
        };

        // act
        await sut.UpdateCinemaOfPicturesAsync(cinemaId, updatedCinema);

        // assert
        repositoryMock.Verify(x => x.UpdateCinemaInfoAsync(cinemaId, updatedCinema), Times.Once());
    }

    [Fact]
    public async Task CheckIfAnyPicturesAssociatedWithCinemaAsync_ShouldCallCheckCinemaAssociationExistenceAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        var cinemaId = "123";

        // act
        await sut.CheckIfAnyPicturesAssociatedWithCinemaAsync(cinemaId);

        // assert
        repositoryMock.Verify(x => x.CheckCinemaAssociationExistenceAsync(cinemaId), Times.Once());
    }

    [Fact]
    public async Task GetActiveYearsAsync_ShouldCallGetActiveYearsAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        // act
        await sut.GetActiveYearsAsync();

        // assert
        repositoryMock.Verify(x => x.GetActiveYearsAsync(), Times.Once());
    }

    [Fact]
    public async Task UploadPictureImageAsync_ShouldCallSaveImageAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePictureService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var yearWatched = 2024;
        var picture = new PictureBuilder(id).WithYearWatched(yearWatched).Build();
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(picture);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("image content"));
        var imageExtension = ".jpg";
        var imageDirectory = "pictures";
        imageStorageMock.Setup(x => x.IsValidImageFormat($"{id}{imageExtension}")).Returns(true);

        // act
        await sut.UploadPictureImageAsync(stream, id, imageExtension, imageDirectory);

        // assert
        imageStorageMock.Verify(
            x => x.SaveImageAsync(stream, "123.jpg", "pictures/2024"),
            Times.Once()
        );
    }

    [Fact]
    public async Task UploadPictureImageAsync_IfImageExtensionIsUnsupported_ShouldThrowImageValidationException()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePictureService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var yearWatched = 2024;
        var picture = new PictureBuilder(id).WithYearWatched(yearWatched).Build();
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(picture);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("image content"));
        var imageExtension = ".txt";
        var imageDirectory = "pictures";
        imageStorageMock.Setup(x => x.IsValidImageFormat($"{id}{imageExtension}")).Returns(false);

        // act
        var act = async () =>
            await sut.UploadPictureImageAsync(stream, id, imageExtension, imageDirectory);

        // assert
        await act.Should().ThrowAsync<ImageValidationException>();
    }

    [Fact]
    public async Task DeletePictureImageAsync_ShouldCallRemoveImageAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePictureService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var yearWatched = 2024;
        var imageUrl = "https://imagehost/images/pictures/2024/123.jpg";
        var picture = new PictureBuilder(id)
            .WithYearWatched(yearWatched)
            .WithImageUrl(imageUrl)
            .Build();
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(picture);

        var imageName = "123.jpg";
        var imageDirectory = "pictures";
        imageStorageMock.Setup(x => x.GetImageFileNameFromUrl(imageUrl)).Returns(imageName);

        // act
        await sut.DeletePictureImageAsync(id, imageDirectory);

        // assert
        imageStorageMock.Verify(x => x.RemoveImageAsync("123.jpg", "pictures/2024"), Times.Once());
    }

    [Fact]
    public async Task DeletePictureImageAsync_IfPictureDoesNotHaveAnImage_ShouldThrowImageValidationException()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePictureService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var yearWatched = 2024;
        var picture = new PictureBuilder(id).WithYearWatched(yearWatched).Build();
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(picture);

        var imageDirectory = "pictures";

        // act
        var act = async () => await sut.DeletePictureImageAsync(id, imageDirectory);

        // assert
        await act.Should().ThrowAsync<ImageValidationException>();
    }

    private static PictureService CreatePictureService(
        IPictureRepository? pictureRepository = null,
        IImageStorage? imageStorage = null
    ) =>
        new PictureService(
            pictureRepository ?? Mock.Of<IPictureRepository>(),
            imageStorage ?? Mock.Of<IImageStorage>(),
            Mock.Of<ILogger<PictureService>>()
        );
}

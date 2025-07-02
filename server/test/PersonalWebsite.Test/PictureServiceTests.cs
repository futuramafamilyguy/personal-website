using System.Collections.Generic;
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
        var pictureBuilder = new PictureBuilder(id);
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(pictureBuilder.Build());

        // act
        var picture = await sut.GetPictureAsync(id);

        // assert
        picture.Id.Should().Be(id);
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

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task AddPictureAsync_ShouldCallAddAsync(bool isFavorite, bool isKino)
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        var name = "cars 2";
        var yearWatched = 2020;
        var picture = new PictureBuilder("123")
            .WithName(name)
            .WithYearWatched(yearWatched)
            .WithFavorite(isFavorite)
            .WithKino(isKino)
            .Build();
        repositoryMock
            .Setup(x => x.CheckKinoPictureExistenceAsync(yearWatched, null))
            .ReturnsAsync(false);

        // act
        await sut.AddPictureAsync(
            picture.Name,
            picture.YearWatched,
            picture.MonthWatched,
            picture.Cinema,
            picture.YearReleased,
            picture.Zinger,
            picture.Alias,
            picture.IsFavorite,
            picture.IsKino,
            picture.IsNewRelease
        );

        // assert
        repositoryMock.Verify(
            x =>
                x.AddAsync(
                    It.Is(
                        (Picture picture) =>
                            picture.Name == name
                            && picture.IsFavorite == isFavorite
                            && picture.IsKino == isKino
                    )
                ),
            Times.Once()
        );
    }

    [Fact]
    public async Task AddPictureAsync_IfKinoWithoutFavourite_ShouldThrowDomainValidationException()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        var yearWatched = 2020;
        var picture = new PictureBuilder("123")
            .WithYearWatched(yearWatched)
            .WithFavorite(false)
            .WithKino(true)
            .Build();
        repositoryMock
            .Setup(x => x.CheckKinoPictureExistenceAsync(yearWatched, null))
            .ReturnsAsync(false);

        // act
        var act = async () =>
            await sut.AddPictureAsync(
                picture.Name,
                picture.YearWatched,
                picture.MonthWatched,
                picture.Cinema,
                picture.YearReleased,
                picture.Zinger,
                picture.Alias,
                picture.IsFavorite,
                picture.IsKino,
                picture.IsNewRelease
            );

        // assert
        await act.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task AddPictureAsync_IfKinoAlreadyExists_ShouldThrowDomainValidationException()
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var sut = CreatePictureService(repositoryMock.Object);

        var yearWatched = 2020;
        var picture = new PictureBuilder("123")
            .WithYearWatched(yearWatched)
            .WithFavorite(true)
            .WithKino(true)
            .Build();
        repositoryMock
            .Setup(x => x.CheckKinoPictureExistenceAsync(yearWatched, null))
            .ReturnsAsync(true);

        // act
        var act = async () =>
            await sut.AddPictureAsync(
                picture.Name,
                picture.YearWatched,
                picture.MonthWatched,
                picture.Cinema,
                picture.YearReleased,
                picture.Zinger,
                picture.Alias,
                picture.IsFavorite,
                picture.IsKino,
                picture.IsNewRelease
            );

        // assert
        await act.Should().ThrowAsync<DomainValidationException>();
    }

    [Theory]
    [InlineData("images/pictures/123.jpg", "images/pictures/123-alt.jpg", 1, 1)]
    [InlineData("images/pictures/123.jpg", null, 1, 0)]
    [InlineData(null, null, 0, 0)]
    public async Task RemovePictureAsync_ShouldCallRemoveAsyncAndDeleteImages(
        string imageObjectKey,
        string altImageObjectKey,
        int expectedDeleteImageCalls,
        int expectedDeleteAltImageCalls
    )
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePictureService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var imageStorageHost = "https://storagehost";
        var picture = new PictureBuilder(id)
            .WithImage(imageStorageHost, imageObjectKey)
            .WithAltImage(imageStorageHost, altImageObjectKey)
            .Build();
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(picture);

        // act
        await sut.RemovePictureAsync(id);

        // assert
        repositoryMock.Verify(x => x.RemoveAsync(id), Times.Once());
        imageStorageMock.Verify(
            x => x.DeleteObjectAsync(imageObjectKey),
            Times.Exactly(expectedDeleteImageCalls)
        );
        imageStorageMock.Verify(
            x => x.DeleteObjectAsync(altImageObjectKey),
            Times.Exactly(expectedDeleteAltImageCalls)
        );
    }

    [Theory]
    [InlineData(false, false, "image/pictures/2020/123.jpg")]
    [InlineData(true, true, "image/pictures/2020/123-alt.jpg")]
    public async Task HandleImageUploadAsync_ShouldReturnPresignedUploadUrl(
        bool isAlt,
        bool isFavorite,
        string expectedImageObjectKey
    )
    {
        // arrange
        var repositoryMock = new Mock<IPictureRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePictureService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var picture = new PictureBuilder(id).WithYearWatched(2020).WithFavorite(isFavorite).Build();
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(picture);
        imageStorageMock
            .Setup(x =>
                x.GeneratePresignedUploadUrlAsync(expectedImageObjectKey, It.IsAny<TimeSpan>())
            )
            .ReturnsAsync("https://storagehost/upload");
        imageStorageMock
            .Setup(x => x.GetPublicUrl(It.IsAny<string>()))
            .Returns((string path) => $"https://storagehost/{path}");

        // act
        var presignedUploadUrl = await sut.HandleImageUploadAsync(
            id,
            "image/pictures",
            "jpg",
            isAlt
        );

        // assert
        repositoryMock.Verify(
            x =>
                x.UpdateImageInfoAsync(
                    id,
                    expectedImageObjectKey,
                    $"https://storagehost/{expectedImageObjectKey}",
                    isAlt
                ),
            Times.Once
        );
        imageStorageMock.Verify(x => x.DeleteObjectAsync(It.IsAny<string>()), Times.Never);
        presignedUploadUrl.Should().Be("https://storagehost/upload");
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

    private static PictureService CreatePictureService(
        IPictureRepository? pictureRepository = null,
        IImageStorage? imageStorage = null
    ) =>
        new PictureService(
            pictureRepository ?? Mock.Of<IPictureRepository>(),
            imageStorage ?? Mock.Of<IImageStorage>()
        );
}

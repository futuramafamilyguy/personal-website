using FluentAssertions;
using Moq;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;
using PersonalWebsite.Core.Services;

namespace PersonalWebsite.Test;

public class PictureCinemaOrchestratorTests
{
    [Fact]
    public async Task AddPictureWithCinemaAsync_ShouldCallAddPictureAsync()
    {
        // arrange
        var pictureServiceMock = new Mock<IPictureService>();
        var cinemaServiceMock = new Mock<ICinemaService>();
        var sut = new PictureCinemaOrchestrator(
            pictureServiceMock.Object,
            cinemaServiceMock.Object);

        var pictureName = "cars";
        var year = 2020;
        var zinger = "bazinga";
        var alias = "car";

        var cinemaId = "123";
        var cinemaName = "Alice";
        var city = "Christchurch";
        var cinema = new Cinema
        {
            Id = cinemaId,
            Name = cinemaName,
            City = city
        };

        cinemaServiceMock.Setup(x => x.GetCinemaAsync(cinemaId))
            .ReturnsAsync(cinema);

        // act
        await sut.AddPictureWithCinemaAsync(pictureName, year, cinemaId, zinger, alias);

        // assert
        cinemaServiceMock.Verify(
            x => x.GetCinemaAsync(cinemaId),
            Times.Once());
        pictureServiceMock.Verify(
            x => x.AddPictureAsync(
                pictureName,
                year,
                It.Is((Cinema cinema) => cinema.Id == cinemaId
                    && cinema.Name == cinemaName
                    && cinema.City == city),
                zinger,
                alias),
            Times.Once());
    }

    [Fact]
    public async Task UpdatePictureWithCinemaAsync_ShouldCallUpdatePictureAsync()
    {
        // arrange
        var pictureServiceMock = new Mock<IPictureService>();
        var cinemaServiceMock = new Mock<ICinemaService>();
        var sut = new PictureCinemaOrchestrator(
            pictureServiceMock.Object,
            cinemaServiceMock.Object);

        var pictureId = "123";
        var pictureName = "cars";
        var year = 2020;
        var zinger = "bazinga";
        var alias = "car";

        var cinemaId = "123";
        var cinemaName = "Alice";
        var city = "Christchurch";
        var cinema = new Cinema
        {
            Id = cinemaId,
            Name = cinemaName,
            City = city
        };

        cinemaServiceMock.Setup(x => x.GetCinemaAsync(cinemaId))
            .ReturnsAsync(cinema);

        // act
        await sut.UpdatePictureWithCinemaAsync(pictureId, pictureName, year, cinemaId, zinger, alias);

        // assert
        cinemaServiceMock.Verify(
            x => x.GetCinemaAsync(cinemaId),
            Times.Once());
        pictureServiceMock.Verify(
            x => x.UpdatePictureAsync(
                pictureId,
                pictureName,
                year,
                cinema,
                zinger,
                alias),
            Times.Once());
    }

    [Fact]
    public async Task UpdateCinemaAndAssociatedPicturesAsync_ShouldCallUpdateCinemaOfPicturesAsync()
    {
        // arrange
        var pictureServiceMock = new Mock<IPictureService>();
        var cinemaServiceMock = new Mock<ICinemaService>();
        var sut = new PictureCinemaOrchestrator(
            pictureServiceMock.Object,
            cinemaServiceMock.Object);

        var cinemaId = "123";
        var updatedCinemaName = "Alice";
        var updatedCity = "Christchurch";
        var updatedCinema = new Cinema
        {
            Id = cinemaId,
            Name = updatedCinemaName,
            City = updatedCity
        };

        cinemaServiceMock.Setup(x => x.UpdateCinemaAsync(cinemaId, updatedCinemaName, updatedCity))
            .ReturnsAsync(updatedCinema);

        // act
        await sut.UpdateCinemaAndAssociatedPicturesAsync(cinemaId, updatedCinemaName, updatedCity);

        // assert
        cinemaServiceMock.Verify(
            x => x.UpdateCinemaAsync(cinemaId, updatedCinemaName, updatedCity),
            Times.Once());
        pictureServiceMock.Verify(
            x => x.UpdateCinemaOfPicturesAsync(cinemaId, updatedCinema),
            Times.Once());
    }

    [Fact]
    public async Task ValidateAndDeleteCinema_WhenNoPicturesAssociatedWithCinemaExist_ShouldCallRemoveCinemaAsync()
    {
        // arrange
        var pictureServiceMock = new Mock<IPictureService>();
        var cinemaServiceMock = new Mock<ICinemaService>();
        var sut = new PictureCinemaOrchestrator(
            pictureServiceMock.Object,
            cinemaServiceMock.Object);

        var cinemaId = "123";

        pictureServiceMock.Setup(x => x.CheckIfAnyPicturesAssociatedWithCinemaAsync(cinemaId))
            .ReturnsAsync(false);


        // act
        await sut.ValidateAndDeleteCinema(cinemaId);

        // assert
        pictureServiceMock.Verify(
            x => x.CheckIfAnyPicturesAssociatedWithCinemaAsync(cinemaId),
            Times.Once());
        cinemaServiceMock.Verify(
            x => x.RemoveCinemaAsync(cinemaId),
            Times.Once());
    }

    [Fact]
    public async Task ValidateAndDeleteCinema_WhenPicturesAssociatedWithCinemaExist_ShouldThrowInvalidOperationException()
    {
        // arrange
        var pictureServiceMock = new Mock<IPictureService>();
        var cinemaServiceMock = new Mock<ICinemaService>();
        var sut = new PictureCinemaOrchestrator(
            pictureServiceMock.Object,
            cinemaServiceMock.Object);

        var cinemaId = "123";

        pictureServiceMock.Setup(x => x.CheckIfAnyPicturesAssociatedWithCinemaAsync(cinemaId))
            .ReturnsAsync(true);
        
        // act
        var action = async () => await sut.ValidateAndDeleteCinema(cinemaId);

        // assert
        await action.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Cinema 123 cannot be deleted because one or more pictures are still associated with the cinema");
    }
}

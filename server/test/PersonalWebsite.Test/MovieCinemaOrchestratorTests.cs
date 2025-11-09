using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;
using PersonalWebsite.Core.Services;

namespace PersonalWebsite.Test;

public class MovieCinemaOrchestratorTests
{
    [Fact]
    public async Task AddMovieWithCinemaAsync_ShouldCallAddMovieAsync()
    {
        // arrange
        var movieServiceMock = new Mock<IMovieService>();
        var cinemaServiceMock = new Mock<ICinemaService>();
        var sut = new MovieCinemaOrchestrator(
            movieServiceMock.Object,
            cinemaServiceMock.Object,
            Mock.Of<ILogger<MovieCinemaOrchestrator>>()
        );

        var movieName = "cars";
        var yearWatched = 2020;
        var monthWatched = Month.Jan;
        int yearReleased = 2020;
        var zinger = "bazinga";
        var alias = "car";
        var motif = "lightning";
        var isFavorite = false;
        var isKino = false;
        var isNewRelease = false;

        var cinemaId = "123";
        var cinemaName = "Alice";
        var city = "Christchurch";
        var cinema = new Cinema
        {
            Id = cinemaId,
            Name = cinemaName,
            City = city
        };

        cinemaServiceMock.Setup(x => x.GetCinemaAsync(cinemaId)).ReturnsAsync(cinema);

        // act
        await sut.AddMovieWithCinemaAsync(
            movieName,
            yearWatched,
            monthWatched,
            cinemaId,
            yearReleased,
            zinger,
            alias,
            motif,
            isFavorite,
            isKino,
            isNewRelease
        );

        // assert
        cinemaServiceMock.Verify(x => x.GetCinemaAsync(cinemaId), Times.Once());
        movieServiceMock.Verify(
            x =>
                x.AddMovieAsync(
                    movieName,
                    yearWatched,
                    monthWatched,
                    It.Is(
                        (Cinema cinema) =>
                            cinema.Id == cinemaId
                            && cinema.Name == cinemaName
                            && cinema.City == city
                    ),
                    yearReleased,
                    zinger,
                    alias,
                    motif,
                    isFavorite,
                    isKino,
                    isNewRelease
                ),
            Times.Once()
        );
    }

    [Fact]
    public async Task UpdateMovieWithCinemaAsync_ShouldCallUpdateMovieAsync()
    {
        // arrange
        var movieServiceMock = new Mock<IMovieService>();
        var cinemaServiceMock = new Mock<ICinemaService>();
        var sut = new MovieCinemaOrchestrator(
            movieServiceMock.Object,
            cinemaServiceMock.Object,
            Mock.Of<ILogger<MovieCinemaOrchestrator>>()
        );

        var movieId = "123";
        var movieName = "cars";
        var yearWatched = 2020;
        var monthWatched = Month.Jan;
        int yearReleased = 2020;
        var zinger = "bazinga";
        var alias = "car";
        var motif = "lightning";
        var imageUrl = "domain/images/image.jpg";
        var imageObjectKey = "images/image.jpg";
        var altImageUrl = "domain/images/image-alt.jpg";
        var altImageObjectKey = "images/image-alt.jpg";
        var isFavorite = false;
        var isKino = false;
        var isNewRelease = false;

        var cinemaId = "123";
        var cinemaName = "Alice";
        var city = "Christchurch";
        var cinema = new Cinema
        {
            Id = cinemaId,
            Name = cinemaName,
            City = city
        };

        cinemaServiceMock.Setup(x => x.GetCinemaAsync(cinemaId)).ReturnsAsync(cinema);

        // act
        await sut.UpdateMovieWithCinemaAsync(
            movieId,
            movieName,
            yearWatched,
            monthWatched,
            cinemaId,
            yearReleased,
            zinger,
            alias,
            motif,
            imageUrl,
            imageObjectKey,
            altImageUrl,
            altImageObjectKey,
            isFavorite,
            isKino,
            isNewRelease
        );

        // assert
        cinemaServiceMock.Verify(x => x.GetCinemaAsync(cinemaId), Times.Once());
        movieServiceMock.Verify(
            x =>
                x.UpdateMovieAsync(
                    movieId,
                    movieName,
                    yearWatched,
                    monthWatched,
                    cinema,
                    yearReleased,
                    zinger,
                    alias,
                    motif,
                    imageUrl,
                    imageObjectKey,
                    altImageUrl,
                    altImageObjectKey,
                    isFavorite,
                    isKino,
                    isNewRelease
                ),
            Times.Once()
        );
    }

    [Fact]
    public async Task UpdateCinemaAndAssociatedMoviesAsync_ShouldCallUpdateCinemaOfMoviesAsync()
    {
        // arrange
        var movieServiceMock = new Mock<IMovieService>();
        var cinemaServiceMock = new Mock<ICinemaService>();
        var sut = new MovieCinemaOrchestrator(
            movieServiceMock.Object,
            cinemaServiceMock.Object,
            Mock.Of<ILogger<MovieCinemaOrchestrator>>()
        );

        var cinemaId = "123";
        var updatedCinemaName = "Alice";
        var updatedCity = "Christchurch";
        var updatedCinema = new Cinema
        {
            Id = cinemaId,
            Name = updatedCinemaName,
            City = updatedCity
        };

        cinemaServiceMock
            .Setup(x => x.UpdateCinemaAsync(cinemaId, updatedCinemaName, updatedCity))
            .ReturnsAsync(updatedCinema);

        // act
        await sut.UpdateCinemaAndAssociatedMoviesAsync(cinemaId, updatedCinemaName, updatedCity);

        // assert
        cinemaServiceMock.Verify(
            x => x.UpdateCinemaAsync(cinemaId, updatedCinemaName, updatedCity),
            Times.Once()
        );
        movieServiceMock.Verify(
            x => x.UpdateCinemaOfMoviesAsync(cinemaId, updatedCinema),
            Times.Once()
        );
    }

    [Fact]
    public async Task ValidateAndDeleteCinema_WhenNoMoviesAssociatedWithCinemaExist_ShouldCallRemoveCinemaAsync()
    {
        // arrange
        var movieServiceMock = new Mock<IMovieService>();
        var cinemaServiceMock = new Mock<ICinemaService>();
        var sut = new MovieCinemaOrchestrator(
            movieServiceMock.Object,
            cinemaServiceMock.Object,
            Mock.Of<ILogger<MovieCinemaOrchestrator>>()
        );

        var cinemaId = "123";

        movieServiceMock
            .Setup(x => x.CheckIfAnyMoviesAssociatedWithCinemaAsync(cinemaId))
            .ReturnsAsync(false);

        // act
        await sut.ValidateAndDeleteCinema(cinemaId);

        // assert
        movieServiceMock.Verify(
            x => x.CheckIfAnyMoviesAssociatedWithCinemaAsync(cinemaId),
            Times.Once()
        );
        cinemaServiceMock.Verify(x => x.RemoveCinemaAsync(cinemaId), Times.Once());
    }

    [Fact]
    public async Task ValidateAndDeleteCinema_WhenMoviesAssociatedWithCinemaExist_ShouldThrowInvalidOperationException()
    {
        // arrange
        var movieServiceMock = new Mock<IMovieService>();
        var cinemaServiceMock = new Mock<ICinemaService>();
        var sut = new MovieCinemaOrchestrator(
            movieServiceMock.Object,
            cinemaServiceMock.Object,
            Mock.Of<ILogger<MovieCinemaOrchestrator>>()
        );

        var cinemaId = "123";

        movieServiceMock
            .Setup(x => x.CheckIfAnyMoviesAssociatedWithCinemaAsync(cinemaId))
            .ReturnsAsync(true);

        // act
        var action = async () => await sut.ValidateAndDeleteCinema(cinemaId);

        // assert
        await action
            .Should()
            .ThrowAsync<DomainValidationException>()
            .WithMessage(
                "cinema cannot be deleted because one or more movies are still associated with the cinema: 123"
            );
    }
}

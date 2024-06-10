using Moq;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;
using PersonalWebsite.Core.Services;

namespace PersonalWebsite.Test;

public class CinemaServiceTests
{
    [Fact]
    public async Task AddCinemaAsync_ShouldCallAddAsync()
    {
        // arrange
        var cinemaRepositoryMock = new Mock<ICinemaRepository>();
        var sut = new CinemaService(cinemaRepositoryMock.Object);

        var name = "Alice";
        var city = "Christchurch";

        // act
        await sut.AddCinemaAsync(name, city);

        // assert
        cinemaRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is((Cinema cinema) => cinema.Name == name
                    && cinema.City == city)),
            Times.Once());
    }

    [Fact]
    public async Task GetCinemasAsync_ShouldCallGetAsync()
    {
        // arrange
        var cinemaRepositoryMock = new Mock<ICinemaRepository>();
        var sut = new CinemaService(cinemaRepositoryMock.Object);

        // act
        await sut.GetCinemasAsync();

        // assert
        cinemaRepositoryMock.Verify(
            x => x.GetAsync(),
            Times.Once());
    }

    [Fact]
    public async Task GetCinemaAsync_ShouldCallGetAsync()
    {
        // arrange
        var cinemaRepositoryMock = new Mock<ICinemaRepository>();
        var sut = new CinemaService(cinemaRepositoryMock.Object);

        var id = "123";

        // act
        await sut.GetCinemaAsync(id);

        // assert
        cinemaRepositoryMock.Verify(
            x => x.GetAsync(id),
            Times.Once());
    }

    [Fact]
    public async Task RemoveCinemaAsync_ShouldCallRemoveAsync()
    {
        // arrange
        var cinemaRepositoryMock = new Mock<ICinemaRepository>();
        var sut = new CinemaService(cinemaRepositoryMock.Object);

        var id = "123";

        // act
        await sut.RemoveCinemaAsync(id);

        // assert
        cinemaRepositoryMock.Verify(
            x => x.RemoveAsync(id),
            Times.Once());
    }

    [Fact]
    public async Task UpdateCinemaAsync_ShouldCallUpdateAsync()
    {
        // arrange
        var cinemaRepositoryMock = new Mock<ICinemaRepository>();
        var sut = new CinemaService(cinemaRepositoryMock.Object);

        var id = "123";
        var name = "Alice";
        var city = "Christchurch";

        // act
        await sut.UpdateCinemaAsync(id, name, city);

        // assert
        cinemaRepositoryMock.Verify(
            x => x.UpdateAsync(
                id,
                It.Is((Cinema cinema) => cinema.Id == id
                    && cinema.Name == name
                    && cinema.City == city)),
            Times.Once());
    }
}

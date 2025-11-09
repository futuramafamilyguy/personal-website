using FluentAssertions;
using Moq;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;
using PersonalWebsite.Core.Services;

namespace PersonalWebsite.Test;

public class MovieServiceTests
{
    [Fact]
    public async Task GetMoviesAsync_ShouldCallGetByYearAsync()
    {
        // arrange
        var repositoryMock = new Mock<IMovieRepository>();
        var sut = CreateMovieService(repositoryMock.Object);

        var yearWatched = 2020;

        var pageNumber = 1;
        var pageSize = int.MaxValue;

        // act
        await sut.GetMoviesAsync(yearWatched, pageNumber, pageSize);

        // assert
        repositoryMock.Verify(
            x => x.GetByYearAsync(yearWatched, pageNumber, pageSize),
            Times.Once()
        );
    }

    [Fact]
    public async Task GetMovieAsync_ShouldCallGetAsync()
    {
        // arrange
        var repositoryMock = new Mock<IMovieRepository>();
        var sut = CreateMovieService(repositoryMock.Object);

        var id = "123";
        var movieBuilder = new MovieBuilder(id);
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(movieBuilder.Build());

        // act
        var movie = await sut.GetMovieAsync(id);

        // assert
        movie.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetFavoriteMoviesAsync_ShouldCallGetFavoriteByYearAsync()
    {
        // arrange
        var repositoryMock = new Mock<IMovieRepository>();
        var sut = CreateMovieService(repositoryMock.Object);

        var yearWatched = 2020;

        // act
        await sut.GetNomineesAsync(yearWatched);

        // assert
        repositoryMock.Verify(x => x.GetNomineesByYearAsync(yearWatched), Times.Once());
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task AddMovieAsync_ShouldCallAddAsync(bool isFavorite, bool isKino)
    {
        // arrange
        var repositoryMock = new Mock<IMovieRepository>();
        var sut = CreateMovieService(repositoryMock.Object);

        var name = "cars 2";
        var yearWatched = 2020;
        var movie = new MovieBuilder("123")
            .WithName(name)
            .WithYearWatched(yearWatched)
            .WithFavorite(isFavorite)
            .WithKino(isKino)
            .Build();
        repositoryMock.Setup(x => x.CheckKinoExistenceAsync(yearWatched, null)).ReturnsAsync(false);

        // act
        await sut.AddMovieAsync(
            movie.Name,
            movie.Year,
            movie.Month,
            movie.Cinema,
            movie.ReleaseYear,
            movie.Zinger,
            movie.Alias,
            movie.Motif,
            movie.IsNominated,
            movie.IsKino,
            movie.IsRetro
        );

        // assert
        repositoryMock.Verify(
            x =>
                x.AddAsync(
                    It.Is(
                        (Movie movie) =>
                            movie.Name == name
                            && movie.IsNominated == isFavorite
                            && movie.IsKino == isKino
                    )
                ),
            Times.Once()
        );
    }

    [Fact]
    public async Task AddMovieAsync_IfKinoWithoutFavourite_ShouldThrowDomainValidationException()
    {
        // arrange
        var repositoryMock = new Mock<IMovieRepository>();
        var sut = CreateMovieService(repositoryMock.Object);

        var yearWatched = 2020;
        var movie = new MovieBuilder("123")
            .WithYearWatched(yearWatched)
            .WithFavorite(false)
            .WithKino(true)
            .Build();
        repositoryMock.Setup(x => x.CheckKinoExistenceAsync(yearWatched, null)).ReturnsAsync(false);

        // act
        var act = async () =>
            await sut.AddMovieAsync(
                movie.Name,
                movie.Year,
                movie.Month,
                movie.Cinema,
                movie.ReleaseYear,
                movie.Zinger,
                movie.Alias,
                movie.Motif,
                movie.IsNominated,
                movie.IsKino,
                movie.IsRetro
            );

        // assert
        await act.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task AddMovieAsync_IfKinoAlreadyExists_ShouldThrowDomainValidationException()
    {
        // arrange
        var repositoryMock = new Mock<IMovieRepository>();
        var sut = CreateMovieService(repositoryMock.Object);

        var yearWatched = 2020;
        var movie = new MovieBuilder("123")
            .WithYearWatched(yearWatched)
            .WithFavorite(true)
            .WithKino(true)
            .Build();
        repositoryMock.Setup(x => x.CheckKinoExistenceAsync(yearWatched, null)).ReturnsAsync(true);

        // act
        var act = async () =>
            await sut.AddMovieAsync(
                movie.Name,
                movie.Year,
                movie.Month,
                movie.Cinema,
                movie.ReleaseYear,
                movie.Zinger,
                movie.Alias,
                movie.Motif,
                movie.IsNominated,
                movie.IsKino,
                movie.IsRetro
            );

        // assert
        await act.Should().ThrowAsync<DomainValidationException>();
    }

    [Theory]
    [InlineData("images/movies/123.jpg", "images/movies/123-alt.jpg", 1, 1)]
    [InlineData("images/movies/123.jpg", null, 1, 0)]
    [InlineData(null, null, 0, 0)]
    public async Task RemoveMovieAsync_ShouldCallRemoveAsyncAndDeleteImages(
        string imageObjectKey,
        string altImageObjectKey,
        int expectedDeleteImageCalls,
        int expectedDeleteAltImageCalls
    )
    {
        // arrange
        var repositoryMock = new Mock<IMovieRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreateMovieService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var imageStorageHost = "https://storagehost";
        var movie = new MovieBuilder(id)
            .WithImage(imageStorageHost, imageObjectKey)
            .WithAltImage(imageStorageHost, altImageObjectKey)
            .Build();
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(movie);

        // act
        await sut.RemoveMovieAsync(id);

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
    [InlineData(false, false, "image/movies/2020/123.jpg")]
    [InlineData(true, true, "image/movies/2020/123-alt.jpg")]
    public async Task HandleImageUploadAsync_ShouldReturnPresignedUploadUrl(
        bool isAlt,
        bool isFavorite,
        string expectedImageObjectKey
    )
    {
        // arrange
        var repositoryMock = new Mock<IMovieRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreateMovieService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var movie = new MovieBuilder(id).WithYearWatched(2020).WithFavorite(isFavorite).Build();
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(movie);
        imageStorageMock
            .Setup(x =>
                x.GeneratePresignedUploadUrlAsync(expectedImageObjectKey, It.IsAny<TimeSpan>())
            )
            .ReturnsAsync("https://storagehost/upload");
        imageStorageMock
            .Setup(x => x.GetPublicUrl(It.IsAny<string>()))
            .Returns((string path) => $"https://storagehost/{path}");

        // act
        var presignedUploadUrl = await sut.HandleImageUploadAsync(id, "image/movies", "jpg", isAlt);

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
    public async Task UpdateCinemaOfMoviesAsync_ShouldCallUpdateCinemaInfoAsync()
    {
        // arrange
        var repositoryMock = new Mock<IMovieRepository>();
        var sut = CreateMovieService(repositoryMock.Object);

        var cinemaId = "123";
        var updatedCinema = new Cinema
        {
            Id = cinemaId,
            Name = "Alice",
            City = "Christchurch"
        };

        // act
        await sut.UpdateCinemaOfMoviesAsync(cinemaId, updatedCinema);

        // assert
        repositoryMock.Verify(x => x.UpdateCinemaInfoAsync(cinemaId, updatedCinema), Times.Once());
    }

    [Fact]
    public async Task CheckIfAnyMoviesAssociatedWithCinemaAsync_ShouldCallCheckCinemaAssociationExistenceAsync()
    {
        // arrange
        var repositoryMock = new Mock<IMovieRepository>();
        var sut = CreateMovieService(repositoryMock.Object);

        var cinemaId = "123";

        // act
        await sut.CheckIfAnyMoviesAssociatedWithCinemaAsync(cinemaId);

        // assert
        repositoryMock.Verify(x => x.CheckCinemaAssociationExistenceAsync(cinemaId), Times.Once());
    }

    [Fact]
    public async Task GetActiveYearsAsync_ShouldCallGetActiveYearsAsync()
    {
        // arrange
        var repositoryMock = new Mock<IMovieRepository>();
        var sut = CreateMovieService(repositoryMock.Object);

        // act
        await sut.GetActiveYearsAsync();

        // assert
        repositoryMock.Verify(x => x.GetActiveYearsAsync(), Times.Once());
    }

    private static MovieService CreateMovieService(
        IMovieRepository? movieRepository = null,
        IImageStorage? imageStorage = null
    ) =>
        new MovieService(
            movieRepository ?? Mock.Of<IMovieRepository>(),
            imageStorage ?? Mock.Of<IImageStorage>()
        );
}

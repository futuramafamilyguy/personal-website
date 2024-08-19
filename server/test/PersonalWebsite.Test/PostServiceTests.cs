using Moq;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;
using PersonalWebsite.Core.Services;

namespace PersonalWebsite.Test;

public class PostServiceTests
{
    [Fact]
    public async Task AddPostAsync_ShouldCallAddAsync()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        var sut = new PostService(postRepositoryMock.Object, dateTimeProviderMock.Object);

        var dateTimeNow = DateTime.UtcNow;
        dateTimeProviderMock.Setup(x => x.UtcNow).Returns(dateTimeNow);

        var title = "Cars Review";

        // act
        await sut.AddPostAsync(title);

        // assert
        postRepositoryMock.Verify(
            x =>
                x.AddAsync(
                    It.Is((Post post) => post.Title == title && post.CreatedAtUtc == dateTimeNow)
                ),
            Times.Once
        );
    }

    [Fact]
    public async Task GetPostAsync_ShouldCallGetAsync()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        var sut = new PostService(postRepositoryMock.Object, dateTimeProviderMock.Object);

        var id = "123";

        // act
        await sut.GetPostAsync(id);

        // assert
        postRepositoryMock.Verify(x => x.GetAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetPostsAsync_ShouldCallGetAsync()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        var sut = new PostService(postRepositoryMock.Object, dateTimeProviderMock.Object);

        // act
        await sut.GetPostsAsync();

        // assert
        postRepositoryMock.Verify(x => x.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task RemovePostAsync_ShouldCallRemoveAsync()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        var sut = new PostService(postRepositoryMock.Object, dateTimeProviderMock.Object);

        var id = "123";

        // act
        await sut.RemovePostAsync(id);

        // assert
        postRepositoryMock.Verify(x => x.RemoveAsync(id), Times.Once);
    }

    [Fact]
    public async Task UpdatePostAsync_ShouldCallUpdateAsync()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        var sut = new PostService(postRepositoryMock.Object, dateTimeProviderMock.Object);

        var dateTimeNow = DateTime.UtcNow;
        dateTimeProviderMock.Setup(x => x.UtcNow).Returns(dateTimeNow);

        var id = "123";
        var title = "Cars 2 Review";
        var contentUrl = "xyz.com/content/123";
        var imageUrl = "xyz.com/image/123";
        var createdAt = new DateTime(2024, 1, 1);

        // act
        await sut.UpdatePostAsync(id, title, contentUrl, imageUrl, createdAt);

        // assert
        postRepositoryMock.Verify(
            x =>
                x.UpdateAsync(
                    id,
                    It.Is(
                        (Post post) =>
                            post.Id == id
                            && post.Title == title
                            && post.ContentUrl == contentUrl
                            && post.ImageUrl == imageUrl
                            && post.LastUpdatedUtc == dateTimeNow
                            && post.CreatedAtUtc == createdAt
                    )
                ),
            Times.Once
        );
    }
}

using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalWebsite.Core.Exceptions;
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
        var sut = CreatePostService(
            postRepository: postRepositoryMock.Object,
            dateTimeProvider: dateTimeProviderMock.Object
        );

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
        var sut = CreatePostService(postRepositoryMock.Object);

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
        var sut = CreatePostService(postRepositoryMock.Object);

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
        var sut = CreatePostService(postRepositoryMock.Object);

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
        var sut = CreatePostService(
            postRepository: postRepositoryMock.Object,
            dateTimeProvider: dateTimeProviderMock.Object
        );

        var dateTimeNow = DateTime.UtcNow;
        dateTimeProviderMock.Setup(x => x.UtcNow).Returns(dateTimeNow);

        var id = "123";
        var title = "Cars 2 Review";
        var contentUrl = "https://storagehost/content/posts/123.jpg";
        var imageUrl = "https://storagehost/images/posts/123.jpg";
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

    [Fact]
    public async Task UploadPostImageAsync_ShouldCallSaveImageAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPostRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePostService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var title = "Cars Review";
        var createdAt = new DateTime(2024, 1, 1);
        var post = new Post
        {
            Id = id,
            Title = title,
            CreatedAtUtc = createdAt,
            LastUpdatedUtc = createdAt
        };
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(post);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("image content"));
        var imageExtension = ".jpg";
        var imageDirectory = "images/posts";
        imageStorageMock.Setup(x => x.IsValidImageFormat($"{id}{imageExtension}")).Returns(true);

        // act
        await sut.UploadPostImageAsync(stream, id, imageExtension, imageDirectory);

        // assert
        imageStorageMock.Verify(
            x => x.SaveImageAsync(stream, "123.jpg", "images/posts"),
            Times.Once()
        );
    }

    [Fact]
    public async Task UploadPostImageAsync_IfImageExtensionIsUnsupported_ShouldThrowImageValidationExceptionl()
    {
        // arrange
        var repositoryMock = new Mock<IPostRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePostService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var title = "Cars Review";
        var createdAt = new DateTime(2024, 1, 1);
        var post = new Post
        {
            Id = id,
            Title = title,
            CreatedAtUtc = createdAt,
            LastUpdatedUtc = createdAt
        };
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(post);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("image content"));
        var imageExtension = ".jpg";
        var imageDirectory = "images/posts";
        imageStorageMock.Setup(x => x.IsValidImageFormat($"{id}{imageExtension}")).Returns(false);

        // act
        var act = async () =>
            await sut.UploadPostImageAsync(stream, id, imageExtension, imageDirectory);

        // assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task DeletePostImageAsync_ShouldCallRemoveImageAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPostRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePostService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var title = "Cars Review";
        var imageUrl = "https://storagehost/images/posts/123.jpg";
        var createdAt = new DateTime(2024, 1, 1);
        var post = new Post
        {
            Id = id,
            Title = title,
            ImageUrl = imageUrl,
            CreatedAtUtc = createdAt,
            LastUpdatedUtc = createdAt
        };
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(post);

        var imageName = "123.jpg";
        var imageDirectory = "images/posts";
        imageStorageMock.Setup(x => x.GetImageFileNameFromUrl(imageUrl)).Returns(imageName);

        // act
        await sut.DeletePostImageAsync(id, imageDirectory);

        // assert
        imageStorageMock.Verify(x => x.RemoveImageAsync("123.jpg", "images/posts"), Times.Once());
    }

    [Fact]
    public async Task DeletePostImageAsync_IfPictureDoesNotHaveAnImage_ShouldThrowValidationException()
    {
        // arrange
        var repositoryMock = new Mock<IPostRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePostService(repositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        var title = "Cars Review";
        var createdAt = new DateTime(2024, 1, 1);
        var post = new Post
        {
            Id = id,
            Title = title,
            CreatedAtUtc = createdAt,
            LastUpdatedUtc = createdAt
        };
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(post);

        var imageDirectory = "images/posts";

        // act
        var act = async () => await sut.DeletePostImageAsync(id, imageDirectory);

        // assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UploadPostContentAsync_ShouldCallSaveMarkdownAsync()
    {
        // arrange
        var markdownStorageMock = new Mock<IMarkdownStorage>();
        var sut = CreatePostService(markdownStorage: markdownStorageMock.Object);

        var id = "123";
        var content = "Cars is a good movie.";
        var directory = "markdown/posts";

        // act
        await sut.UploadPostContentAsync(content, id, directory);

        // assert
        markdownStorageMock.Verify(
            x => x.SaveMarkdownAsync(content, "123.md", directory),
            Times.Once()
        );
    }

    [Fact]
    public async Task DeletePostContentAsync_ShouldCallRemoveMarkdownAsync()
    {
        // arrange
        var repositoryMock = new Mock<IPostRepository>();
        var markdownStorageMock = new Mock<IMarkdownStorage>();
        var sut = CreatePostService(
            repositoryMock.Object,
            markdownStorage: markdownStorageMock.Object
        );

        var id = "123";
        var title = "Cars Review";
        var contentUrl = "https://storagehost/markdown/posts/123.md";
        var createdAt = new DateTime(2024, 1, 1);
        var post = new Post
        {
            Id = id,
            Title = title,
            ContentUrl = contentUrl,
            CreatedAtUtc = createdAt,
            LastUpdatedUtc = createdAt
        };
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(post);

        var markdownName = "123.md";
        var directory = "markdown/posts";
        markdownStorageMock
            .Setup(x => x.GetMarkdownFileNameFromUrl(contentUrl))
            .Returns(markdownName);

        // act
        await sut.DeletePostContentAsync(id, directory);

        // assert
        markdownStorageMock.Verify(
            x => x.RemoveMarkdownAsync("123.md", "markdown/posts"),
            Times.Once()
        );
    }

    [Fact]
    public async Task DeletePostContentAsync_IfPostDoesNotHaveContent_ShouldThrowValidationException()
    {
        // arrange
        var repositoryMock = new Mock<IPostRepository>();
        var markdownStorageMock = new Mock<IMarkdownStorage>();
        var sut = CreatePostService(
            repositoryMock.Object,
            markdownStorage: markdownStorageMock.Object
        );

        var id = "123";
        var title = "Cars Review";
        var createdAt = new DateTime(2024, 1, 1);
        var post = new Post
        {
            Id = id,
            Title = title,
            CreatedAtUtc = createdAt,
            LastUpdatedUtc = createdAt
        };
        repositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(post);

        var directory = "markdown/posts";

        // act
        var act = async () => await sut.DeletePostContentAsync(id, directory);

        // assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    private static PostService CreatePostService(
        IPostRepository? postRepository = null,
        IImageStorage? imageStorage = null,
        IMarkdownStorage? markdownStorage = null,
        IDateTimeProvider? dateTimeProvider = null
    ) =>
        new PostService(
            postRepository ?? Mock.Of<IPostRepository>(),
            imageStorage ?? Mock.Of<IImageStorage>(),
            markdownStorage ?? Mock.Of<IMarkdownStorage>(),
            dateTimeProvider ?? Mock.Of<IDateTimeProvider>(),
            Mock.Of<ILogger<PostService>>()
        );
}

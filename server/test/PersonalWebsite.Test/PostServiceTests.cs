using FluentAssertions;
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
        var slug = "cars-review";

        // act
        await sut.AddPostAsync(title);

        // assert
        postRepositoryMock.Verify(
            x =>
                x.AddAsync(
                    It.Is(
                        (Post post) =>
                            post.Title == title
                            && post.CreatedAtUtc == dateTimeNow
                            && post.Slug == slug
                            && post.MarkdownVersion == 0
                    )
                ),
            Times.Once
        );
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
    public async Task GetPostAsync_ShouldReturnPost()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var sut = CreatePostService(postRepositoryMock.Object);

        var id = "123";
        var title = "Cars Review";
        postRepositoryMock
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(
                new Post
                {
                    Id = id,
                    Title = title,
                    Slug = "cars-review",
                    CreatedAtUtc = new DateTime(2024, 1, 1),
                    LastUpdatedUtc = new DateTime(2024, 1, 2),
                    MarkdownVersion = 1
                }
            );

        // act
        var post = await sut.GetPostAsync(id);

        // assert
        post.Id.Should().Be(id);
        post.Title.Should().Be(title);
    }

    [Fact]
    public async Task GetPostAsync_IfIdDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var sut = CreatePostService(postRepositoryMock.Object);

        var id = "123";
        postRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync((Post?)null);

        // act
        var act = async () => await sut.GetPostAsync(id);

        // assert
        await act.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task GetPostBySlugAsync_ShouldCallGetBySlugAsync()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var sut = CreatePostService(postRepositoryMock.Object);

        var id = "123";
        var slug = "cars-review";
        postRepositoryMock
            .Setup(x => x.GetBySlugAsync(slug))
            .ReturnsAsync(
                new Post
                {
                    Id = id,
                    Title = "Cars Review",
                    CreatedAtUtc = new DateTime(2024, 1, 1),
                    LastUpdatedUtc = new DateTime(2024, 1, 1),
                    Slug = slug,
                    MarkdownVersion = 1
                }
            );

        // act
        var post = await sut.GetPostBySlugAsync(slug);

        // assert
        post.Id.Should().Be(id);
        post.Slug.Should().Be(slug);
    }

    [Fact]
    public async Task GetPostBySlugAsync_IfSlugDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var sut = CreatePostService(postRepositoryMock.Object);

        var slug = "cars-revie";
        postRepositoryMock.Setup(x => x.GetBySlugAsync(slug)).ReturnsAsync((Post?)null);

        // act
        var act = async () => await sut.GetPostBySlugAsync(slug);

        // assert
        await act.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task RemovePostAsync_ShouldCallRemoveAsyncAndDeleteRelatedMedia()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var markdownStorageMock = new Mock<IMarkdownStorage>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePostService(
            postRepositoryMock.Object,
            imageStorageMock.Object,
            markdownStorageMock.Object
        );

        var id = "123";
        var markdownObjectKey = "markdown/posts/123-v1.md";
        var imageObjectKey = "image/posts/123.md";
        postRepositoryMock
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(
                new Post
                {
                    Id = "123",
                    Title = "Cars Review",
                    MarkdownUrl = "https://storagehost/markdown/posts/123-v1.md",
                    MarkdownObjectKey = markdownObjectKey,
                    ImageUrl = "https://storagehost/images/posts/123.jpg",
                    ImageObjectKey = imageObjectKey,
                    CreatedAtUtc = new DateTime(2024, 1, 1),
                    LastUpdatedUtc = new DateTime(2024, 1, 1),
                    Slug = "cars-review",
                    MarkdownVersion = 1
                }
            );

        // act
        await sut.RemovePostAsync(id);

        // assert
        postRepositoryMock.Verify(x => x.RemoveAsync(id), Times.Once);
        markdownStorageMock.Verify(x => x.ArchiveObjectAsync(markdownObjectKey), Times.Once);
        imageStorageMock.Verify(x => x.DeleteObjectAsync(imageObjectKey), Times.Once);
    }

    [Fact]
    public async Task HandleImageUploadAsync_ShouldReturnedPresignedUploadUrl()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePostService(postRepositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        postRepositoryMock
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(
                new Post
                {
                    Id = "123",
                    Title = "Cars Review",
                    CreatedAtUtc = new DateTime(2024, 1, 1),
                    LastUpdatedUtc = new DateTime(2024, 1, 1),
                    Slug = "cars-review",
                    MarkdownVersion = 1
                }
            );
        var imageBasePath = "image/posts";
        var extension = "jpg";
        imageStorageMock
            .Setup(x => x.GetPublicUrl(It.IsAny<string>()))
            .Returns((string path) => $"https://storagehost/{path}");
        imageStorageMock
            .Setup(x =>
                x.GeneratePresignedUploadUrlAsync("image/posts/123.jpg", It.IsAny<TimeSpan>())
            )
            .ReturnsAsync("https://storagehost/upload");

        // act
        var presignedUploadUrl = await sut.HandleImageUploadAsync(id, imageBasePath, extension);

        // assert
        postRepositoryMock.Verify(
            x =>
                x.UpdateImageInfoAsync(
                    id,
                    "image/posts/123.jpg",
                    "https://storagehost/image/posts/123.jpg"
                ),
            Times.Once
        );
        imageStorageMock.Verify(x => x.DeleteObjectAsync(It.IsAny<string>()), Times.Never);
        presignedUploadUrl.Should().Be("https://storagehost/upload");
    }

    [Fact]
    public async Task HandleImageUploadAsync_IfPostHasExistingImage_ShouldDeleteImage()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var imageStorageMock = new Mock<IImageStorage>();
        var sut = CreatePostService(postRepositoryMock.Object, imageStorageMock.Object);

        var id = "123";
        postRepositoryMock
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(
                new Post
                {
                    Id = "123",
                    Title = "Cars Review",
                    ImageUrl = "https://storagehost/images/posts/123.jpg",
                    ImageObjectKey = "images/posts/123.jpg",
                    CreatedAtUtc = new DateTime(2024, 1, 1),
                    LastUpdatedUtc = new DateTime(2024, 1, 1),
                    Slug = "cars-review",
                    MarkdownVersion = 1
                }
            );
        var imageBasePath = "image/posts";
        var extension = "jpg";
        imageStorageMock
            .Setup(x => x.GetPublicUrl(It.IsAny<string>()))
            .Returns((string path) => $"https://storagehost/{path}");
        imageStorageMock
            .Setup(x =>
                x.GeneratePresignedUploadUrlAsync("image/posts/123.jpg", It.IsAny<TimeSpan>())
            )
            .ReturnsAsync("https://storagehost/upload");

        // act
        var presignedUploadUrl = await sut.HandleImageUploadAsync(id, imageBasePath, extension);

        // assert
        postRepositoryMock.Verify(
            x =>
                x.UpdateImageInfoAsync(
                    id,
                    "image/posts/123.jpg",
                    "https://storagehost/image/posts/123.jpg"
                ),
            Times.Once
        );
        imageStorageMock.Verify(x => x.DeleteObjectAsync("images/posts/123.jpg"), Times.Once);
        presignedUploadUrl.Should().Be("https://storagehost/upload");
    }

    [Fact]
    public async Task HandleMarkdownUploadAsync_ShouldReturnedPresignedUploadUrl()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var markdownStorageMock = new Mock<IMarkdownStorage>();
        var sut = CreatePostService(
            postRepositoryMock.Object,
            markdownStorage: markdownStorageMock.Object
        );

        var id = "123";
        var currentVersion = 0;
        var newVersion = 1;
        postRepositoryMock
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(
                new Post
                {
                    Id = "123",
                    Title = "Cars Review",
                    CreatedAtUtc = new DateTime(2024, 1, 1),
                    LastUpdatedUtc = new DateTime(2024, 1, 1),
                    Slug = "cars-review",
                    MarkdownVersion = currentVersion
                }
            );
        postRepositoryMock.Setup(x => x.IncrementMarkdownVersionAsync(id)).ReturnsAsync(newVersion);
        var markdownBasePath = "markdown/posts";
        markdownStorageMock
            .Setup(x => x.GetPublicUrl(It.IsAny<string>()))
            .Returns((string path) => $"https://storagehost/{path}");
        markdownStorageMock
            .Setup(x =>
                x.GeneratePresignedUploadUrlAsync("markdown/posts/123-v1.md", It.IsAny<TimeSpan>())
            )
            .ReturnsAsync("https://storagehost/upload");

        // act
        var presignedUploadUrl = await sut.HandleMarkdownUploadAsync(id, markdownBasePath);

        // assert
        postRepositoryMock.Verify(
            x =>
                x.UpdateMarkdownInfoAsync(
                    id,
                    "markdown/posts/123-v1.md",
                    "https://storagehost/markdown/posts/123-v1.md"
                ),
            Times.Once
        );
        markdownStorageMock.Verify(x => x.DeleteObjectAsync(It.IsAny<string>()), Times.Never);
        presignedUploadUrl.Should().Be("https://storagehost/upload");
    }

    [Fact]
    public async Task HandleMarkdownUploadAsync_IfMarkdownVersionGreaterThan1_ShouldDeletePreviousMarkdownl()
    {
        // arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var markdownStorageMock = new Mock<IMarkdownStorage>();
        var sut = CreatePostService(
            postRepositoryMock.Object,
            markdownStorage: markdownStorageMock.Object
        );

        var id = "123";
        var currentVersion = 1;
        var newVersion = 2;
        postRepositoryMock
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(
                new Post
                {
                    Id = "123",
                    Title = "Cars Review",
                    CreatedAtUtc = new DateTime(2024, 1, 1),
                    LastUpdatedUtc = new DateTime(2024, 1, 1),
                    Slug = "cars-review",
                    MarkdownVersion = currentVersion
                }
            );
        postRepositoryMock.Setup(x => x.IncrementMarkdownVersionAsync(id)).ReturnsAsync(newVersion);
        var markdownBasePath = "markdown/posts";
        markdownStorageMock
            .Setup(x => x.GetPublicUrl(It.IsAny<string>()))
            .Returns((string path) => $"https://storagehost/{path}");
        markdownStorageMock
            .Setup(x =>
                x.GeneratePresignedUploadUrlAsync(
                    $"markdown/posts/123-v{newVersion}.md",
                    It.IsAny<TimeSpan>()
                )
            )
            .ReturnsAsync("https://storagehost/upload");

        // act
        var presignedUploadUrl = await sut.HandleMarkdownUploadAsync(id, markdownBasePath);

        // assert
        postRepositoryMock.Verify(
            x =>
                x.UpdateMarkdownInfoAsync(
                    id,
                    $"markdown/posts/123-v{newVersion}.md",
                    $"https://storagehost/markdown/posts/123-v{newVersion}.md"
                ),
            Times.Once
        );
        markdownStorageMock.Verify(
            x => x.DeleteObjectAsync($"markdown/posts/123-v{currentVersion}.md"),
            Times.Once
        );
        presignedUploadUrl.Should().Be("https://storagehost/upload");
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
            dateTimeProvider ?? Mock.Of<IDateTimeProvider>()
        );
}

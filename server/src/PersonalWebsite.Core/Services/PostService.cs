using Microsoft.Extensions.Logging;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IImageStorage _imageStorage;
    private readonly IMarkdownStorage _markdownStorage;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<PostService> _logger;

    public PostService(
        IPostRepository postRepository,
        IImageStorage imageStorage,
        IMarkdownStorage markdownStorage,
        IDateTimeProvider dateTimeProvider,
        ILogger<PostService> logger
    )
    {
        _postRepository = postRepository;
        _imageStorage = imageStorage;
        _markdownStorage = markdownStorage;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Post> AddPostAsync(string title)
    {
        var post = await _postRepository.AddAsync(
            new Post
            {
                Title = title,
                LastUpdatedUtc = _dateTimeProvider.UtcNow,
                CreatedAtUtc = _dateTimeProvider.UtcNow,
            }
        );

        return post;
    }

    public async Task<Post> GetPostAsync(string id) => await _postRepository.GetAsync(id);

    public async Task<IEnumerable<Post>> GetPostsAsync() => await _postRepository.GetAsync();

    public async Task RemovePostAsync(string id) => await _postRepository.RemoveAsync(id);

    public async Task<Post> UpdatePostAsync(
        string id,
        string title,
        string? contentUrl,
        string? imageUrl,
        DateTime createdAtUtc
    )
    {
        var updatedPost = await _postRepository.UpdateAsync(
            id,
            new Post
            {
                Id = id,
                Title = title,
                ContentUrl = contentUrl,
                ImageUrl = imageUrl,
                LastUpdatedUtc = _dateTimeProvider.UtcNow,
                CreatedAtUtc = createdAtUtc,
            }
        );

        return updatedPost;
    }

    public async Task<string> UploadPostImageAsync(
        Stream imageStream,
        string id,
        string imageExtension,
        string imageDirectory
    )
    {
        try
        {
            var imageName = $"{id}{imageExtension}";
            if (!_imageStorage.IsValidImageFormat(imageName))
            {
                _logger.LogError($"Image extension '{imageExtension}' not supported");
                throw new ValidationException("Invalid image extension");
            }

            var imageUrl = await _imageStorage.SaveImageAsync(imageStream, imageName, imageDirectory);

            return imageUrl;
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Image validation failure encountered");
            throw;
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "An error occurred while saving the image");
            throw;
        }
    }

    public async Task DeletePostImageAsync(string id, string imageDirectory)
    {
        var post = await GetPostAsync(id);

        try
        {
            if (post.ImageUrl is null)
            {
                _logger.LogError($"Post '{id}' does not have an image that can be deleted");
                throw new ValidationException("No image associated with post");
            }

            var imageFileName = _imageStorage.GetImageFileNameFromUrl(post.ImageUrl!);
            await _imageStorage.RemoveImageAsync(imageFileName, imageDirectory);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Image validation failure encountered");
            throw;
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the image");
            throw;
        }
    }

    public async Task<string> UploadPostContentAsync(string content, string id, string contentDirectory)
    {
        try
        {
            var fileName = $"{id}.md";
            var markdownUrl = await _markdownStorage.SaveMarkdownAsync(content, fileName, contentDirectory);

            return markdownUrl;
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "An error occurred while saving post content");
            throw;
        }
    }

    public async Task DeletePostContentAsync(string id, string contentDirectory)
    {
        var post = await GetPostAsync(id);

        try
        {
            if (post.ContentUrl is null)
            {
                _logger.LogError($"Post '{id}' does not have content that can be deleted");
                throw new ValidationException("No content associated with post");
            }

            var contentFileName = _markdownStorage.GetMarkdownFileNameFromUrl(post.ContentUrl!);
            await _markdownStorage.RemoveMarkdownAsync(contentFileName, contentDirectory);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Post content validation failure encountered");
            throw;
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting post content");
            throw;
        }
    }
}

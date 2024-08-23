using Microsoft.Extensions.Logging;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IImageStorage _imageStorage;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<PostService> _logger;

    public PostService(
        IPostRepository postRepository,
        IImageStorage imageStorage,
        IDateTimeProvider dateTimeProvider,
        ILogger<PostService> logger
    )
    {
        _postRepository = postRepository;
        _imageStorage = imageStorage;
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
                throw new ImageValidationException("Invalid image extension");
            }

            await _imageStorage.SaveImageAsync(imageStream, imageName, imageDirectory);

            var imageUrl = _imageStorage.GetImageUrl(imageName, imageDirectory);

            return imageUrl;
        }
        catch (ImageValidationException ex)
        {
            _logger.LogError(ex, "Image validation failure encountered");
            throw;
        }
        catch (ImageStorageException ex)
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
                throw new ImageValidationException("No image associated with post");
            }

            var imageFileName = _imageStorage.GetImageFileNameFromUrl(post.ImageUrl!);
            await _imageStorage.RemoveImageAsync(imageFileName, imageDirectory);
        }
        catch (ImageValidationException ex)
        {
            _logger.LogError(ex, "Image validation failure encountered");
            throw;
        }
        catch (ImageStorageException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the image");
            throw;
        }
    }
}

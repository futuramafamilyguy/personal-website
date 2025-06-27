using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class  PostService : IPostService
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
        var utcNow = _dateTimeProvider.UtcNow;
        var post = await _postRepository.AddAsync(
            new Post
            {
                Title = title,
                LastUpdatedUtc = utcNow,
                CreatedAtUtc = utcNow,
                Slug = GenerateSlug(title)
            }
        );

        return post;
    }

    public async Task<Post> GetPostAsync(string id) => await _postRepository.GetAsync(id);

    public async Task<IEnumerable<Post>> GetPostsAsync() => await _postRepository.GetAsync();

    public async Task<Post> GetPostBySlugAsync(string slug)
    {
        var post = await _postRepository.GetBySlugAsync(slug);

        if (post is null)
        {
            throw new EntityNotFoundException($"Post entity with slug '{slug}' not found");
        }

        return post;
    }

    public async Task RemovePostAsync(string id)
    {
        var post = await _postRepository.GetAsync(id);
        if (!string.IsNullOrEmpty(post.ImageObjectKey))
            await _imageStorage.DeleteObjectAsync(post.ImageObjectKey);

        await _postRepository.RemoveAsync(id);
    }

    public async Task<Post> UpdatePostAsync(
        string id,
        string title,
        string? contentUrl,
        string? imageUrl,
        string? imageObjectKey,
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
                ImageObjectKey = imageObjectKey,
                LastUpdatedUtc = _dateTimeProvider.UtcNow,
                CreatedAtUtc = createdAtUtc,
                Slug = GenerateSlug(title)
            }
        );

        return updatedPost;
    }

    public async Task<string> HandleImageUploadAsync(
        string id,
        string imageBasePath,
        string fileExtension
    )
    {
        var post = await _postRepository.GetAsync(id);
        if (!string.IsNullOrEmpty(post.ImageObjectKey))
            await _imageStorage.DeleteObjectAsync(post.ImageObjectKey);

        var objectKey = $"{imageBasePath}/{id}.{fileExtension}";
        var presignedUrl = await _imageStorage.GeneratePresignedUploadUrlAsync(
            objectKey,
            TimeSpan.FromMinutes(5)
        );
        var publicUrl = _imageStorage.GetPublicUrl(objectKey);

        await _postRepository.UpdateImageAsync(id, objectKey, publicUrl);

        return presignedUrl;
    }

    public async Task<string> UploadPostContentAsync(
        string content,
        string id,
        string contentDirectory
    )
    {
        try
        {
            var fileName = $"{id}.md";
            var markdownUrl = await _markdownStorage.SaveMarkdownAsync(
                content,
                fileName,
                contentDirectory
            );

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

            await _markdownStorage.CopyMarkdownAsync(
                contentFileName,
                GenerateArchivedFileName(contentFileName),
                contentDirectory
            );
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

    private static string GenerateArchivedFileName(string fileName) =>
        $"{Path.GetFileNameWithoutExtension(fileName)}-archived{Path.GetExtension(fileName)}";

    private static string GenerateSlug(string title) =>
        Regex
            .Replace(
                title.Split(':').Length > 1 ? title.Split(':')[1].Trim() : title.Trim(),
                @"[^a-zA-Z0-9\s-]",
                ""
            )
            .ToLower()
            .Replace(" ", "-");
}

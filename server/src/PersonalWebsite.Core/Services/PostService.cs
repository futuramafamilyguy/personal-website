using System.Text.RegularExpressions;
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

    public PostService(
        IPostRepository postRepository,
        IImageStorage imageStorage,
        IMarkdownStorage markdownStorage,
        IDateTimeProvider dateTimeProvider
    )
    {
        _postRepository = postRepository;
        _imageStorage = imageStorage;
        _markdownStorage = markdownStorage;
        _dateTimeProvider = dateTimeProvider;
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
                Slug = GenerateSlug(title),
                MarkdownVersion = 0,
            }
        );

        return post;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync() => await _postRepository.GetAsync();

    public async Task<Post> GetPostAsync(string id)
    {
        var post = await _postRepository.GetAsync(id);
        if (post is null)
            throw new EntityNotFoundException($"post not found: {id}");

        return post;
    }

    public async Task<Post> GetPostBySlugAsync(string slug)
    {
        var post = await _postRepository.GetBySlugAsync(slug);
        if (post is null)
            throw new EntityNotFoundException($"post not found: {slug}");

        return post;
    }

    public async Task RemovePostAsync(string id)
    {
        var post = await _postRepository.GetAsync(id);
        if (post is null)
            throw new EntityNotFoundException($"post not found: {id}");

        if (!string.IsNullOrEmpty(post.MarkdownObjectKey))
            await _markdownStorage.ArchiveObjectAsync(post.MarkdownObjectKey);
        if (!string.IsNullOrEmpty(post.ImageObjectKey))
            await _imageStorage.DeleteObjectAsync(post.ImageObjectKey);

        await _postRepository.RemoveAsync(id);
    }

    public async Task<Post> UpdatePostAsync(
        string id,
        string title,
        string? markdownUrl,
        string? markdownObjectKey,
        string? imageUrl,
        string? imageObjectKey,
        DateTime createdAtUtc,
        int markdownVersion
    )
    {
        var updatedPost = new Post
        {
            Id = id,
            Title = title,
            MarkdownUrl = markdownUrl,
            MarkdownObjectKey = markdownObjectKey,
            ImageUrl = imageUrl,
            ImageObjectKey = imageObjectKey,
            LastUpdatedUtc = _dateTimeProvider.UtcNow,
            CreatedAtUtc = createdAtUtc,
            Slug = GenerateSlug(title),
            MarkdownVersion = markdownVersion,
        };
        var result = await _postRepository.UpdateAsync(id, updatedPost);
        if (!result)
            throw new EntityNotFoundException($"post not found: {id}");

        return updatedPost;
    }

    public async Task<string> HandleImageUploadAsync(
        string id,
        string imageBasePath,
        string fileExtension
    )
    {
        var post = await _postRepository.GetAsync(id);
        if (post is null)
            throw new EntityNotFoundException($"post not found: {id}");

        if (!string.IsNullOrEmpty(post.ImageObjectKey))
            await _imageStorage.DeleteObjectAsync(post.ImageObjectKey);

        var objectKey = $"{imageBasePath}/{id}.{fileExtension}";
        var presignedUrl = await _imageStorage.GeneratePresignedUploadUrlAsync(
            objectKey,
            TimeSpan.FromMinutes(5)
        );
        var publicUrl = _imageStorage.GetPublicUrl(objectKey);

        await _postRepository.UpdateImageInfoAsync(id, objectKey, publicUrl);

        return presignedUrl;
    }

    public async Task<string> HandleMarkdownUploadAsync(string id, string markdownBasePath)
    {
        var newMarkdownVersion = await _postRepository.IncrementMarkdownVersionAsync(id);
        var objectKey = $"{markdownBasePath}/{id}-v{newMarkdownVersion}.md";
        var presignedUrl = await _markdownStorage.GeneratePresignedUploadUrlAsync(
            objectKey,
            TimeSpan.FromMinutes(5)
        );
        var publicUrl = _markdownStorage.GetPublicUrl(objectKey);

        if (newMarkdownVersion > 1)
        {
            var prevObjectKey = $"{markdownBasePath}/{id}-v{newMarkdownVersion - 1}.md";
            await _markdownStorage.DeleteObjectAsync(prevObjectKey);
        }

        await _postRepository.UpdateMarkdownInfoAsync(id, objectKey, publicUrl);

        return presignedUrl;
    }

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

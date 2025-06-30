using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Infrastructure.Data.Posts;

public static class PostMapper
{
    public static Post ToDomain(PostDocument document) =>
        new Post
        {
            Id = document.Id,
            Title = document.Title,
            MarkdownUrl = document.MarkdownUrl,
            MarkdownObjectKey = document.MarkdownObjectKey,
            ImageUrl = document.ImageUrl,
            ImageObjectKey = document.ImageObjectKey,
            LastUpdatedUtc = document.LastUpdatedUtc,
            CreatedAtUtc = document.CreatedAtUtc,
            Slug = document.Slug,
        };

    public static PostDocument ToDocument(Post post) =>
        new PostDocument
        {
            Id = post.Id,
            Title = post.Title,
            MarkdownUrl = post.MarkdownUrl,
            MarkdownObjectKey = post.MarkdownObjectKey,
            ImageUrl = post.ImageUrl,
            ImageObjectKey = post.ImageObjectKey,
            LastUpdatedUtc = post.LastUpdatedUtc,
            CreatedAtUtc = post.CreatedAtUtc,
            Slug = post.Slug,
        };
}

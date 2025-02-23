﻿using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Infrastructure.Data.Posts;

public static class PostMapper
{
    public static Post ToDomain(PostDocument document) =>
        new Post
        {
            Id = document.Id,
            Title = document.Title,
            ContentUrl = document.ContentUrl,
            ImageUrl = document.ImageUrl,
            LastUpdatedUtc = document.LastUpdatedUtc,
            CreatedAtUtc = document.CreatedAtUtc,
            Slug = document.Slug,
        };

    public static PostDocument ToDocument(Post post) =>
        new PostDocument
        {
            Id = post.Id,
            Title = post.Title,
            ContentUrl = post.ContentUrl,
            ImageUrl = post.ImageUrl,
            LastUpdatedUtc = post.LastUpdatedUtc,
            CreatedAtUtc = post.CreatedAtUtc,
            Slug = post.Slug,
        };
}

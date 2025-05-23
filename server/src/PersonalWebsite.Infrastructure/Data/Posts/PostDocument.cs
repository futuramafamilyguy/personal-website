﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PersonalWebsite.Infrastructure.Data.Posts;

public class PostDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("post_title")]
    public required string Title { get; set; }

    [BsonElement("content_url")]
    public string? ContentUrl { get; set; }

    [BsonElement("image_url")]
    public string? ImageUrl { get; set; }

    [BsonElement("last_updated")]
    public required DateTime LastUpdatedUtc { get; set; }

    [BsonElement("created_at")]
    public required DateTime CreatedAtUtc { get; set; }

    [BsonElement("slug")]
    public required string Slug { get; set; }
}

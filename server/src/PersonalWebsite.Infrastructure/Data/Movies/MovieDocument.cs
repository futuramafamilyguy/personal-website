using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PersonalWebsite.Core.Enums;
using PersonalWebsite.Infrastructure.Data.Cinemas;

namespace PersonalWebsite.Infrastructure.Data.Movies;

public class MovieDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("year")]
    public required int Year { get; set; }

    [BsonElement("month")]
    public required Month Month { get; set; }

    [BsonElement("release_year")]
    public required int ReleaseYear { get; set; }

    [BsonElement("cinema")]
    public required CinemaDocument Cinema { get; set; }

    [BsonElement("nominated")]
    public bool IsNominated { get; set; }

    [BsonElement("kino")]
    public bool IsKino { get; set; }

    [BsonElement("retro")]
    public bool IsRetro { get; set; }

    [BsonElement("alias")]
    public string? Alias { get; set; }

    [BsonElement("zinger")]
    public string? Zinger { get; set; }

    [BsonElement("motif")]
    public string? Motif { get; set; }

    [BsonElement("image_url")]
    public string? ImageUrl { get; set; }

    [BsonElement("image_object_key")]
    public string? ImageObjectKey { get; set; }

    [BsonElement("alt_image_url")]
    public string? AltImageUrl { get; set; }

    [BsonElement("alt_image_object_key")]
    public string? AltImageObjectKey { get; set; }
}

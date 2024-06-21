using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PersonalWebsite.Infrastructure.Data.Cinemas;

namespace PersonalWebsite.Infrastructure.Data.Pictures;

public class PictureDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("picture_name")]
    public required string Name { get; set; }

    [BsonElement("year_watched")]
    public required int Year { get; set; }

    [BsonElement("cinema")]
    public required CinemaDocument Cinema { get; set; }

    [BsonElement("zinger")]
    public string? Zinger { get; set; }

    [BsonElement("favorite")]
    public bool IsFavorite { get; set; }

    [BsonElement("picture_alias")]
    public string? Alias { get; set; }

    [BsonElement("image_url")]
    public string? ImageUrl { get; set; }
}

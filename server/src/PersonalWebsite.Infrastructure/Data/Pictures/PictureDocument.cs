using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PersonalWebsite.Core.Enums;
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
    public required int YearWatched { get; set; }

    [BsonElement("month_watched")]
    public required Month MonthWatched { get; set; }

    [BsonElement("cinema")]
    public required CinemaDocument Cinema { get; set; }

    [BsonElement("year_released")]
    public required int YearReleased { get; set; }

    [BsonElement("zinger")]
    public string? Zinger { get; set; }

    [BsonElement("favorite")]
    public bool IsFavorite { get; set; }

    [BsonElement("picture_alias")]
    public string? Alias { get; set; }

    [BsonElement("image_url")]
    public string? ImageUrl { get; set; }

    [BsonElement("alt_image_url")]
    public string? AltImageUrl { get; set; }
}

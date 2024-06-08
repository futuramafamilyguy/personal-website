using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

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

    [BsonElement("zinger")]
    public string? Zinger { get; set; }
}

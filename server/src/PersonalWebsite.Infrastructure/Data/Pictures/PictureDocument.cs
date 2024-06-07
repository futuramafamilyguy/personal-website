using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PersonalWebsite.Infrastructure.Data.Pictures;

public class PictureDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("pictureName")]
    public required string Name { get; set; }

    [BsonElement("yearWatched")]
    public required int Year { get; set; }

    [BsonElement("zinger")]
    public string? Zinger { get; set; }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PersonalWebsite.Infrastructure.Data.Cinemas;

public class CinemaDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("city")]
    public required string City { get; set; }
}

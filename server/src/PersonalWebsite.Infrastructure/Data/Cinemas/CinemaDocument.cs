using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PersonalWebsite.Infrastructure.Data.Cinemas;

public class CinemaDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("cinema_name")]
    public required string Name { get; set; }

    [BsonElement("city")]
    public required string City { get; set; }
}

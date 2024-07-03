using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PersonalWebsite.Infrastructure.Data.Visits;

public class VisitStatistics
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("total_visits")]
    public required int TotalVisits { get; set; }

    [BsonElement("latest_visit")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? LatestVisitUtc { get; set; }

    [BsonElement("tracking_start")]
    [BsonRepresentation(BsonType.DateTime)]
    public required DateTime TrackingStartUtc { get; set; }
}

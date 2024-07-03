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
    public DateTimeOffset? LatestVisit { get; set; }

    [BsonElement("start_tracking")]
    [BsonRepresentation(BsonType.DateTime)]
    public required DateTimeOffset StartTrackingDate { get; set; }
}

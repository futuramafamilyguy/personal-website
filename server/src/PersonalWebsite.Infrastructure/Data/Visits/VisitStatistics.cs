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
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTimeOffset? LatestVisit { get; set; }

    [BsonElement("start_tracking")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public required DateTimeOffset StartTrackingDate { get; set; }
}

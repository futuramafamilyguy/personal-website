namespace PersonalWebsite.Infrastructure.Data;

public class MongoDbConfiguration
{
    public required string DatabaseName { get; set; }
    public required string PicturesCollectionName { get; set; }
    public required string CinemasCollectionName { get; set; }
    public required string PostsCollectionName { get; set; }
    public required string VisitStatisticsCollectionName { get; set; }
}

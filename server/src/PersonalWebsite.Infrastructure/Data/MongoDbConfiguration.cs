namespace PersonalWebsite.Infrastructure.Data;

public class MongoDbConfiguration
{
    public required string DatabaseName { get; set; }
    public required string PicturesCollectionName { get; set; }
}

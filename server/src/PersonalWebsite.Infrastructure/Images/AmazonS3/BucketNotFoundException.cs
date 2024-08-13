namespace PersonalWebsite.Infrastructure.Images.AmazonS3;

public class BucketNotFoundException : Exception
{
    public string Bucket { get; }

    public BucketNotFoundException(string bucketName)
    {
        Bucket = bucketName;
    }

    public BucketNotFoundException(string bucketName, string message)
        : base(message)
    {
        Bucket = bucketName;
    }

    public BucketNotFoundException(string bucketName, string message, Exception inner)
        : base(message, inner)
    {
        Bucket = bucketName;
    }
}

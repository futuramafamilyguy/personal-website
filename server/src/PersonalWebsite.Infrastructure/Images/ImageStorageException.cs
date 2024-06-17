namespace PersonalWebsite.Infrastructure.Images;

public class ImageStorageException : Exception
{
    public ImageStorageException() { }

    public ImageStorageException(string message) : base(message) { }

    public ImageStorageException(string message, Exception inner) : base(message, inner) { }
}

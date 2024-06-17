namespace PersonalWebsite.Infrastructure.Images;

public class InvalidImageFormatException : Exception
{
    public InvalidImageFormatException() { }

    public InvalidImageFormatException(string message) : base(message) { }

    public InvalidImageFormatException(string message, Exception inner) : base(message, inner) { }
}

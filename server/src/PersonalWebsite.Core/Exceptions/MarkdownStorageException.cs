namespace PersonalWebsite.Core.Exceptions;

public class MarkdownStorageException : Exception
{
    public MarkdownStorageException() { }

    public MarkdownStorageException(string message)
        : base(message) { }

    public MarkdownStorageException(string message, Exception inner)
        : base(message, inner) { }
}

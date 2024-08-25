namespace PersonalWebsite.Core.Exceptions;

public class StorageException : Exception
{
    public StorageException() { }

    public StorageException(string message)
        : base(message) { }

    public StorageException(string message, Exception inner)
        : base(message, inner) { }
}

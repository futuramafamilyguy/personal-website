namespace PersonalWebsite.Core.Exceptions;

public class CinemaHasAssociatedPicturesException : Exception
{
    public CinemaHasAssociatedPicturesException() { }

    public CinemaHasAssociatedPicturesException(string message)
        : base(message) { }
}

﻿namespace PersonalWebsite.Core.Exceptions;

public class ImageValidationException : Exception
{
    public ImageValidationException() { }

    public ImageValidationException(string message)
        : base(message) { }

    public ImageValidationException(string message, Exception inner)
        : base(message, inner) { }
}

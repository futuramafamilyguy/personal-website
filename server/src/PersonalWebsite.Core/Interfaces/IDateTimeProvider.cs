﻿namespace PersonalWebsite.Core.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
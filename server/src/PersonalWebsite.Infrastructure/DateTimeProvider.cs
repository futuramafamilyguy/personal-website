using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Infrastructure;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}

namespace PersonalWebsite.Api;

public class RateLimitingConfiguration
{
    public int PermitLimit { get; set; }
    public int WindowMinutes { get; set; }
    public int QueueLimit { get; set; }
}

namespace PersonalWebsite.Api.Authentication;

public class BasicAuthConfiguration
{
    public required string AdminUsername { get; set; }
    public required string AdminPassword { get; set; }
    public required string DisableTrackingUsername { get; set; }
    public required string DisableTrackingPassword { get; set; }
}

namespace PersonalWebsite.Core.Models;

public class Picture
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required int Year { get; set; }
    public string? Zinger { get; set; }
}

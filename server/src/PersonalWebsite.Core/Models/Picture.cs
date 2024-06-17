namespace PersonalWebsite.Core.Models;

public class Picture
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required int Year { get; set; }
    public required Cinema Cinema { get; set; }
    public string? Zinger { get; set; }
    public bool IsFavorite { get; set; } = false;
    public string? Alias { get; set; }
    public string? ImageUrl { get; set; }
}

using PersonalWebsite.Core.Enums;

namespace PersonalWebsite.Core.Models;

public class Movie
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required int Year { get; set; }
    public required Month Month { get; set; }
    public required int ReleaseYear { get; set; }
    public required Cinema Cinema { get; set; }
    public required bool IsNominated { get; set; }
    public required bool IsKino { get; set; }
    public required bool IsRetro { get; set; }
    public string? Alias { get; set; }
    public string? Zinger { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageObjectKey { get; set; }
    public string? AltImageUrl { get; set; }
    public string? AltImageObjectKey { get; set; }
}

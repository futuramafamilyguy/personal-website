namespace PersonalWebsite.Api.DTOs;

public class GenerateImageUploadUrlRequest
{
    public required string FileExtension { get; set; }
    public bool? IsAlt { get; set; }
}

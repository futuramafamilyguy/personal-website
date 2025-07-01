using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.Api.DTOs;

public class CreatePostRequest
{
    [MinLength(1)]
    public required string Title { get; set; }
}

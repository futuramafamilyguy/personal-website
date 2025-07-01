using System.ComponentModel.DataAnnotations;

namespace PersonalWebsite.Api.DTOs;

public class CreateCinemaRequest
{
    [MinLength(1)]
    public required string Name { get; set; }

    [MinLength(1)]
    public required string City { get; set; }
}

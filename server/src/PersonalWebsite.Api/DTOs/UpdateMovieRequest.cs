﻿using System.ComponentModel.DataAnnotations;
using PersonalWebsite.Core.Enums;

namespace PersonalWebsite.Api.DTOs;

public class UpdateMovieRequest
{
    [MinLength(1)]
    public required string Name { get; set; }
    public required int Year { get; set; }
    public Month? Month { get; set; }
    public required string CinemaId { get; set; }
    public int? ReleaseYear { get; set; }
    public string? Zinger { get; set; }
    public string? Alias { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageObjectKey { get; set; }
    public string? AltImageUrl { get; set; }
    public string? AltImageObjectKey { get; set; }
    public bool? IsNominated { get; set; }
    public bool? IsKino { get; set; }
    public bool? IsRetro { get; set; }
}

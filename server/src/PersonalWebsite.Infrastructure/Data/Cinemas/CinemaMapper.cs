using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Infrastructure.Data.Cinemas;

public class CinemaMapper
{
    public static Cinema ToDomain(CinemaDocument document)
        => new Cinema
        {
            Id = document.Id,
            Name = document.Name,
            City = document.City
        };

    public static CinemaDocument ToDocument(Cinema cinema)
        => new CinemaDocument
        {
            Id = cinema.Id,
            Name = cinema.Name,
            City = cinema.City 
        };
}

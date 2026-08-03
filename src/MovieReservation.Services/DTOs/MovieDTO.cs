using System.ComponentModel.DataAnnotations;
using MovieReservation.Domain;

namespace MovieReservation.Services;


public record MovieCreateRequest(
    [Required] string Name,
    [Required] Genre Genre,
    [MaxLength(Movie.DescriptionMaxLength)] string? Description
);

public record MovieUpdateRequest(
    [Required] long Id,
    string? Name,
    Genre? Genre,
    [MaxLength(Movie.DescriptionMaxLength)] string? Description
);

public record MovieResponse(long Id, string Name, Genre Genre, string? Description)
{
    public static MovieResponse FromMovie(Movie movie) =>
        new(movie.Id, movie.Name, movie.Genre, movie.Description);
}
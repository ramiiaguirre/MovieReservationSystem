using System.ComponentModel.DataAnnotations;
using MovieReservation.Domain;

namespace MovieReservation.Services;


public record MovieCreateRequest(
    [Required] string Name,
    [MaxLength(Movie.DescriptionMaxLength)] string? Description
);

public record MovieUpdateRequest(
    [Required] long? Id,
    string? Name,
    [MaxLength(Movie.DescriptionMaxLength)] string? Description
);

public record MovieResponse(long Id, string Name, string? Description)
{
    public static MovieResponse FromMovie(Movie movie) =>
        new(movie.Id, movie.Name, movie.Description);
}
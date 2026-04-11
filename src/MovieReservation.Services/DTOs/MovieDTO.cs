using System.ComponentModel.DataAnnotations;

namespace MovieReservation.Services;

public class MovieDTO
{
    public long Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Length(0, 120)]
    public string? Description { get; set; }
}
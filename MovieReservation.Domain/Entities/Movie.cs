namespace MovieReservation.Domain;

public class Movie
{
    public long Id { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; } = default!;

    public DateTime ShowTime { get; set; }
}

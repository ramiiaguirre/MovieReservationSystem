namespace MovieReservation.Model.Domain;

public class Movie
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
}
namespace MovieReservation.API.View;

public class MovieDTO
{
    public long Id { get; set; } = default!;
    public string Title { get; set; }  = default!;
    public string? Description { get; set; }
}
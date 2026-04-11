namespace MovieReservation.Domain;

public class TheaterRoom
{
    public long Id { get; set; }

    public string Name { get; private set; } = default!;

    public string? Description { get; private set; } = default!;

    public string RoomType { get; set; } = default!;

    public int Capacity { get; private set; }
}

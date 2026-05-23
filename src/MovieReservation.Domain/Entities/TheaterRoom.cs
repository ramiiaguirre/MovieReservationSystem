namespace MovieReservation.Domain;

public class TheaterRoom
{
    public long Id { get; set; }

    public string Name { get; private set; } = default!;

    public string? Description { get; private set; }

    public string RoomType { get; set; } = default!;

    public int Capacity { get; private set; }

    public virtual ICollection<Seat>? Seats { get; set; }

    public TheaterRoom(string name, string roomType, int capacity)
    {
        Name = name;
        RoomType = roomType;
        Capacity = capacity;
    }
}

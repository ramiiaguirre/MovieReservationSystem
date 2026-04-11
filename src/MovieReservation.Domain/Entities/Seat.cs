namespace MovieReservation.Domain;

public class Seat
{
    public long Id { get; set; }

    public string RowLetter { get; private set; } = default!;

    public int SeatNumber { get; private set; }

    public string SeatType { get; set; } = default!;

    public bool IsActive { get; private set; }
}

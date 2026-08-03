namespace MovieReservation.Domain;

public class ReservationSeat
{
    public long Id { get; set; }

    public long ReservationId { get; set; }
    public Reservation Reservation { get; set; } = default!;

    public long SeatId { get; set; }
    public Seat Seat { get; set; } = default!;

    public long ShowTimeId { get; set; }
    public ShowTime ShowTime { get; set; } = default!;

    public decimal Price { get; private set; }

    public ReservationSeat(long reservationId, long seatId, long showTimeId, decimal price)
    {
        ReservationId = reservationId;
        SeatId = seatId;
        ShowTimeId = showTimeId;
        Price = price;
    }
}

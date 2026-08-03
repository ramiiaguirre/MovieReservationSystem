using System;

namespace MovieReservation.Domain;

public class ShowTime
{
    public long Id { get; set; }

    public long MovieId { get; set; }
    public Movie Movie { get; set; } = default!;

    public long TheaterRoomId { get; set; }
    public TheaterRoom TheaterRoom { get; set; } = default!;

    public DateTime ShowDateTime { get; set; }

    public TimeSpan Duration { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ReservationSeat>? ReservationSeats { get; set; }

    public ShowTime(long movieId, long theaterRoomId, DateTime showDateTime, TimeSpan duration, decimal price)
    {
        MovieId = movieId;
        TheaterRoomId = theaterRoomId;
        ShowDateTime = showDateTime;
        Duration = duration;
        Price = price;
        IsActive = true;
    }
}

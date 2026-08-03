namespace MovieReservation.Domain;

public class Reservation
{
    public long Id { get; set; }

    public long UserId { get; set; }
    public User User { get; set; } = default!;

    public long ShowTimeId { get; set; }
    public ShowTime ShowTime { get; set; } = default!;

    public string ReservationCode { get; private set; } = default!;

    public decimal TotalAmount { get; private set; }

    public string Status { get; set; } = default!;

    public DateTime CreatedAt { get; private set; }

    public DateTime? PaymentDate { get; private set; }

    public virtual ICollection<ReservationSeat> Seats { get; set; } = new List<ReservationSeat>();

    public Reservation(long userId, long showTimeId, decimal totalAmount)
    {
        UserId = userId;
        ShowTimeId = showTimeId;
        TotalAmount = totalAmount;
        Status = "pending";
        CreatedAt = DateTime.UtcNow;
        ReservationCode = $"RES-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";
    }

    public void Confirm()
    {
        Status = "confirmed";
        PaymentDate = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = "cancelled";
    }
}

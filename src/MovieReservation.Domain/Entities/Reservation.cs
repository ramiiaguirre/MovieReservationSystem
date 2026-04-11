namespace MovieReservation.Domain;

public class Reservation
{
    public long Id { get; set; }

    public string ReservationCode { get; private set; } = default!;

    public decimal TotalAmount { get; private set; }

    public string Status { get; set; } = default!;

    public DateTime PaymentDate { get; private set; }

}

using System.ComponentModel.DataAnnotations;
using MovieReservation.Domain;

namespace MovieReservation.Services;

public record ReservationCreateRequest(
    [Required] long ShowTimeId,
    [Required][MinLength(1)] List<long> SeatIds
);

public record ReservationSeatResponse(long SeatId, string RowLetter, int SeatNumber, decimal Price);

public record ReservationResponse(
    long Id,
    string ReservationCode,
    long UserId,
    long ShowTimeId,
    decimal TotalAmount,
    string Status,
    DateTime CreatedAt,
    DateTime? PaymentDate,
    List<ReservationSeatResponse> Seats
);

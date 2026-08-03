using System.ComponentModel.DataAnnotations;
using MovieReservation.Domain;

namespace MovieReservation.Services;

public record ShowTimeCreateRequest(
    [Required] long MovieId,
    [Required] long TheaterRoomId,
    [Required] DateTime ShowDateTime,
    [Required] TimeSpan Duration,
    [Required] decimal Price
);

public record ShowTimeUpdateRequest(
    [Required] long Id,
    DateTime? ShowDateTime,
    TimeSpan? Duration,
    decimal? Price,
    bool? IsActive
);

public record ShowTimeResponse(
    long Id,
    long MovieId,
    string MovieName,
    long TheaterRoomId,
    string TheaterRoomName,
    DateTime ShowDateTime,
    TimeSpan Duration,
    decimal Price,
    bool IsActive
)
{
    public static ShowTimeResponse FromShowTime(ShowTime showTime) =>
        new(
            showTime.Id,
            showTime.MovieId,
            showTime.Movie?.Name ?? string.Empty,
            showTime.TheaterRoomId,
            showTime.TheaterRoom?.Name ?? string.Empty,
            showTime.ShowDateTime,
            showTime.Duration,
            showTime.Price,
            showTime.IsActive
        );
}

public record SeatAvailabilityResponse(
    long SeatId,
    string RowLetter,
    int SeatNumber,
    string SeatType,
    bool IsAvailable
);

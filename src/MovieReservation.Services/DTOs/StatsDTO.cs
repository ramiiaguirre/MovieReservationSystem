namespace MovieReservation.Services;

public record MovieStatsResponse(
    long MovieId,
    string MovieName,
    int TotalShowtimes,
    int TotalReservedSeats,
    int TotalCapacity,
    decimal OccupancyPercentage
);

public record ShowtimeStatsResponse(
    long ShowtimeId,
    int Capacity,
    int ReservedSeats,
    decimal OccupancyPercentage,
    decimal Revenue
);

public record OccupancyByDateResponse(
    DateTime Date,
    int TotalCapacity,
    int TotalReservedSeats,
    decimal OccupancyPercentage
);

public record RevenueByRangeResponse(
    DateTime From,
    DateTime To,
    decimal TotalRevenue,
    int TotalReservations
);

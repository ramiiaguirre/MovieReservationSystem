using Microsoft.EntityFrameworkCore;

namespace MovieReservation.Services;

// Inyecta MovieReservationContext directamente (no IRepository<T>): requiere
// agregaciones/joins entre Reservation, ReservationSeat, ShowTime, Seat y TheaterRoom
// que el repositorio genérico no soporta.
public class StatsService : IStatsService
{
    private readonly MovieReservationContext _context;

    public StatsService(MovieReservationContext context)
    {
        _context = context;
    }

    public async Task<MovieStatsResponse?> GetMovieStats(long movieId)
    {
        var movie = await _context.Movies.FindAsync(movieId);
        if (movie is null)
            return null;

        var showTimes = await _context.ShowTimes
            .Where(st => st.MovieId == movieId)
            .Include(st => st.TheaterRoom)
            .ToListAsync();

        var showTimeIds = showTimes.Select(st => st.Id).ToList();

        var reservedSeats = showTimeIds.Count > 0
            ? await _context.ReservationSeats.Where(rs => showTimeIds.Contains(rs.ShowTimeId)).CountAsync()
            : 0;

        var totalCapacity = showTimes.Sum(st => st.TheaterRoom.Capacity);
        var occupancy = totalCapacity > 0 ? Math.Round((decimal)reservedSeats / totalCapacity * 100, 2) : 0m;

        return new MovieStatsResponse(movieId, movie.Name, showTimes.Count, reservedSeats, totalCapacity, occupancy);
    }

    public async Task<ShowtimeStatsResponse?> GetShowtimeStats(long showtimeId)
    {
        var showTime = await _context.ShowTimes
            .Include(st => st.TheaterRoom)
            .FirstOrDefaultAsync(st => st.Id == showtimeId);

        if (showTime is null)
            return null;

        var reservationSeats = await _context.ReservationSeats
            .Where(rs => rs.ShowTimeId == showtimeId)
            .ToListAsync();

        var reservedSeats = reservationSeats.Count;
        var capacity = showTime.TheaterRoom.Capacity;
        var occupancy = capacity > 0 ? Math.Round((decimal)reservedSeats / capacity * 100, 2) : 0m;
        var revenue = reservationSeats.Sum(rs => rs.Price);

        return new ShowtimeStatsResponse(showtimeId, capacity, reservedSeats, occupancy, revenue);
    }

    public async Task<OccupancyByDateResponse> GetOccupancyByDate(DateTime date)
    {
        var showTimes = await _context.ShowTimes
            .Where(st => st.ShowDateTime.Date == date.Date)
            .Include(st => st.TheaterRoom)
            .ToListAsync();

        var showTimeIds = showTimes.Select(st => st.Id).ToList();

        var reservedSeats = showTimeIds.Count > 0
            ? await _context.ReservationSeats.Where(rs => showTimeIds.Contains(rs.ShowTimeId)).CountAsync()
            : 0;

        var totalCapacity = showTimes.Sum(st => st.TheaterRoom.Capacity);
        var occupancy = totalCapacity > 0 ? Math.Round((decimal)reservedSeats / totalCapacity * 100, 2) : 0m;

        return new OccupancyByDateResponse(date.Date, totalCapacity, reservedSeats, occupancy);
    }

    public async Task<RevenueByRangeResponse> GetRevenueByRange(DateTime from, DateTime to)
    {
        var reservations = await _context.Reservations
            .Where(r => r.CreatedAt.Date >= from.Date && r.CreatedAt.Date <= to.Date && r.Status == "confirmed")
            .ToListAsync();

        var totalRevenue = reservations.Sum(r => r.TotalAmount);

        return new RevenueByRangeResponse(from.Date, to.Date, totalRevenue, reservations.Count);
    }
}

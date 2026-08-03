namespace MovieReservation.Services;

public interface IStatsService
{
    Task<MovieStatsResponse?> GetMovieStats(long movieId);
    Task<ShowtimeStatsResponse?> GetShowtimeStats(long showtimeId);
    Task<OccupancyByDateResponse> GetOccupancyByDate(DateTime date);
    Task<RevenueByRangeResponse> GetRevenueByRange(DateTime from, DateTime to);
}

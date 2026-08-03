using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Services;

namespace MovieReservation.API;

[Route("api/stats")]
[ApiController]
[Authorize(Roles = "Admin")]
public class StatsController : ControllerBase
{
    private readonly IStatsService _statsService;
    private readonly ILogger<StatsController> _logger;

    public StatsController(IStatsService statsService, ILogger<StatsController> logger)
    {
        _statsService = statsService;
        _logger = logger;
    }

    [HttpGet]
    [Route("movies/{id}")]
    public async Task<Results<Ok<MovieStatsResponse>, NotFound<ProblemDetails>>> GetMovieStats(long id)
    {
        var stats = await _statsService.GetMovieStats(id);

        if (stats is null)
        {
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Not Found",
                Detail = $"Movie {id} not found"
            });
        }

        return TypedResults.Ok(stats);
    }

    [HttpGet]
    [Route("showtime/{id}")]
    public async Task<Results<Ok<ShowtimeStatsResponse>, NotFound<ProblemDetails>>> GetShowtimeStats(long id)
    {
        var stats = await _statsService.GetShowtimeStats(id);

        if (stats is null)
        {
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Not Found",
                Detail = $"Showtime {id} not found"
            });
        }

        return TypedResults.Ok(stats);
    }

    [HttpGet]
    [Route("occupancy")]
    public async Task<Ok<OccupancyByDateResponse>> GetOccupancy([FromQuery] DateTime date)
    {
        var stats = await _statsService.GetOccupancyByDate(date);
        return TypedResults.Ok(stats);
    }

    [HttpGet]
    [Route("revenue")]
    public async Task<Ok<RevenueByRangeResponse>> GetRevenue([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var stats = await _statsService.GetRevenueByRange(from, to);
        return TypedResults.Ok(stats);
    }
}

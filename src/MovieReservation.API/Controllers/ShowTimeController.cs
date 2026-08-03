using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Services;

namespace MovieReservation.API;

[Route("api/showtimes")]
[ApiController]
public class ShowTimeController : ControllerBase
{
    private readonly IShowTimeService _showTimeService;
    private readonly ILogger<ShowTimeController> _logger;

    public ShowTimeController(IShowTimeService showTimeService, ILogger<ShowTimeController> logger)
    {
        _showTimeService = showTimeService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<Results<
        Created<ShowTimeResponse>,
        Conflict<ProblemDetails>>>
        CreateShowTime([FromBody] ShowTimeCreateRequest request)
    {
        try
        {
            var showTimeCreated = await _showTimeService.Create(request);
            return TypedResults.Created($"api/showtimes/{showTimeCreated.Id}", showTimeCreated);
        }
        catch (Exception e)
        {
            return TypedResults.Conflict(new ProblemDetails()
            {
                Title = "Conflict",
                Detail = e.Message
            });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<Results<
        Ok<ShowTimeResponse>,
        NotFound<ProblemDetails>,
        Conflict<ProblemDetails>>>
        UpdateShowTime([FromBody] ShowTimeUpdateRequest request)
    {
        try
        {
            var showTimeUpdated = await _showTimeService.Update(request);

            if (showTimeUpdated is null)
            {
                return TypedResults.NotFound(new ProblemDetails()
                {
                    Title = "Showtime Not Found",
                    Detail = "No showtime was found with the provided data",
                    Status = StatusCodes.Status404NotFound
                });
            }

            return TypedResults.Ok(showTimeUpdated);
        }
        catch (Exception e)
        {
            return TypedResults.Conflict(new ProblemDetails()
            {
                Title = "Conflict",
                Detail = e.Message
            });
        }
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<Results<
        NoContent,
        NotFound<ProblemDetails>,
        Conflict<ProblemDetails>>>
        DeleteShowTime(long id)
    {
        try
        {
            bool isDeleted = await _showTimeService.Delete(id);

            if (isDeleted)
                return TypedResults.NoContent();

            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Showtime Not Found",
                Detail = "No showtime was found with the provided data",
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (Exception e)
        {
            return TypedResults.Conflict(new ProblemDetails()
            {
                Title = "Conflict",
                Detail = e.Message
            });
        }
    }

    [HttpGet]
    public async Task<Ok<List<ShowTimeResponse>>> Get([FromQuery] long? movieId, [FromQuery] DateTime? date)
    {
        var showTimes = await _showTimeService.GetAll(movieId, date);
        return TypedResults.Ok(showTimes.ToList());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Results<Ok<ShowTimeResponse>, NotFound<ProblemDetails>>> GetShowTime(long id)
    {
        var showTime = await _showTimeService.GetById(id);

        if (showTime is null)
        {
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Not Found",
                Detail = $"Showtime {id} not found"
            });
        }

        return TypedResults.Ok(showTime);
    }

    [HttpGet]
    [Route("{id}/seats")]
    public async Task<Results<Ok<List<SeatAvailabilityResponse>>, NotFound<ProblemDetails>>> GetSeats(long id)
    {
        var seats = await _showTimeService.GetSeatsAvailability(id);

        if (seats is null)
        {
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Not Found",
                Detail = $"Showtime {id} not found"
            });
        }

        return TypedResults.Ok(seats.ToList());
    }
}

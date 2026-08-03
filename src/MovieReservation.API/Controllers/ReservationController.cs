using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Services;

namespace MovieReservation.API;

[Route("api/reservations")]
[ApiController]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationController> _logger;

    public ReservationController(IReservationService reservationService, ILogger<ReservationController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<Results<
        Created<ReservationResponse>,
        Conflict<ProblemDetails>>>
        CreateReservation([FromBody] ReservationCreateRequest request)
    {
        try
        {
            var reservationCreated = await _reservationService.Create(this.GetCurrentUserId(), request);
            return TypedResults.Created($"api/reservations/{reservationCreated.Id}", reservationCreated);
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
    public async Task<Ok<List<ReservationResponse>>> Get([FromQuery] long? userId)
    {
        var reservations = await _reservationService.List(this.GetCurrentUserId(), this.IsAdmin(), userId);
        return TypedResults.Ok(reservations.ToList());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Results<Ok<ReservationResponse>, NotFound<ProblemDetails>, ForbidHttpResult>> GetReservation(long id)
    {
        try
        {
            var reservation = await _reservationService.GetById(id, this.GetCurrentUserId(), this.IsAdmin());

            if (reservation is null)
            {
                return TypedResults.NotFound(new ProblemDetails()
                {
                    Title = "Not Found",
                    Detail = $"Reservation {id} not found"
                });
            }

            return TypedResults.Ok(reservation);
        }
        catch (UnauthorizedAccessException)
        {
            return TypedResults.Forbid();
        }
    }

    [HttpPut]
    [Route("{id}/cancel")]
    public async Task<Results<
        Ok<ReservationResponse>,
        NotFound<ProblemDetails>,
        ForbidHttpResult,
        Conflict<ProblemDetails>>>
        CancelReservation(long id)
    {
        try
        {
            var reservation = await _reservationService.Cancel(id, this.GetCurrentUserId(), this.IsAdmin());

            if (reservation is null)
            {
                return TypedResults.NotFound(new ProblemDetails()
                {
                    Title = "Not Found",
                    Detail = $"Reservation {id} not found"
                });
            }

            return TypedResults.Ok(reservation);
        }
        catch (UnauthorizedAccessException)
        {
            return TypedResults.Forbid();
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

    [HttpGet("~/api/user/{userId}/reservations")]
    [Authorize(Roles = "Admin")]
    public async Task<Ok<List<ReservationResponse>>> GetUserReservations(long userId)
    {
        var reservations = await _reservationService.GetByUser(userId);
        return TypedResults.Ok(reservations.ToList());
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Domain;
using MovieReservation.Services;

namespace MovieReservation.API;

[Route("api/movies")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IShowTimeService _showTimeService;
    private readonly ILogger<MovieController> _logger;
    public MovieController(IMovieService movieService, IShowTimeService showTimeService, ILogger<MovieController> logger)
    {
        _movieService = movieService;
        _showTimeService = showTimeService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<Results<Created<MovieResponse>,
        BadRequest<ProblemDetails>, 
        Conflict<ProblemDetails>>> 
        CreateMovie([FromBody] MovieCreateRequest request)
    {
        try
        {
            var movieCreated = await _movieService.CreateMovie(request);
            
            if (!string.IsNullOrEmpty(movieCreated.Name))
            {   
                return TypedResults.Created($"api/movies/{movieCreated.Id}", movieCreated);
            }
            else
            {   
                return TypedResults.BadRequest(new ProblemDetails()
                {
                    Title = "Bad Request",
                    Detail = "The resource could not be created."
                });
            }
        }
        catch(Exception e)
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
        Ok<MovieResponse>,
        NotFound<ProblemDetails>, 
        Conflict<ProblemDetails>>> 
        UpdateMovie([FromBody] MovieUpdateRequest request)
    {

        var movieUpdated = await _movieService.UpdateMovie(request);

        if (movieUpdated is null)
        {       
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Movie Not Found",
                Detail = $"No movie was found with the provided data",
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            });
        }

        return TypedResults.Ok(movieUpdated);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<Results<
        NoContent,
        NotFound<ProblemDetails>, 
        Conflict<ProblemDetails>>> 
        DeleteMovie(long id)
    {
        try
        {
            bool isMovieDeleted = await _movieService.DeleteMovie(id);

            if (isMovieDeleted)
                return TypedResults.NoContent();
            else
                return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Movie Not Found",
                Detail = $"No movie was found with the provided data",
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            });
        }
        catch(Exception e)
        {
            return TypedResults.Conflict(new ProblemDetails()
            {
                Title = "Conflict",
                Detail = e.Message
            });
        }
    }

    [HttpGet]
    public async Task<Results<Ok<List<MovieResponse>>, NotFound<ProblemDetails>>> Get([FromQuery] Genre? genre)
    {
        var movies = await _movieService.GetMovies(genre);
        if (movies is null)
        {
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Not Found",
                Detail = "There are no movies"
            });
        }

        return TypedResults.Ok(movies.ToList());
    }


    [HttpGet]
    [Route("{id}")]
    public async Task<Results<Ok<MovieResponse>, NotFound<ProblemDetails>, Conflict<ProblemDetails>>> GetMovie(long id)
    {
        _logger.LogDebug("Fetching movie with ID {MovieId}", id);
        var movie = await _movieService.GetMovie(id);

        if (movie is null)
        {
            _logger.LogInformation("Movie ID {MovieId} not found", id);
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Not Found",
                Detail = $"Movie {id} not found"
            });
        }

        return TypedResults.Ok(movie);

    }

    [HttpGet]
    [Route("{id}/showtimes")]
    public async Task<Results<Ok<List<ShowTimeResponse>>, NotFound<ProblemDetails>>> GetShowtimes(long id, [FromQuery] DateTime? date)
    {
        var movie = await _movieService.GetMovie(id);
        if (movie is null)
        {
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Not Found",
                Detail = $"Movie {id} not found"
            });
        }

        var showTimes = await _showTimeService.GetAll(movieId: id, date: date);
        return TypedResults.Ok(showTimes.ToList());
    }

}
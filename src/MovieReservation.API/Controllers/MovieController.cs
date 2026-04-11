using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Services;

namespace MovieReservation.API;

[Route("api/movies")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }
	
	// - [GET]		/movies?{filters} 							#List filtering movies. For example: genre
	// - [GET]		/movies/{id_movie}/showtimes				#See showtimes of the movie
	// - [GET]		/movies/{id_movie}/showtimes?date={date}	#See showtimes of the movie for a specific date

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<Results<Created<ApiResponse<MovieResponse>>, BadRequest<ProblemDetails>, Conflict<ProblemDetails>>> CreateMovie([FromBody] MovieCreateRequest request)
    {
        try
        {
            var movieCreated = await _movieService.CreateMovie(request);
            
            if (!string.IsNullOrEmpty(movieCreated.Name))
            {   
                return TypedResults.Created($"api/movies/{movieCreated.Id}", ApiResponse<MovieResponse>.Success(movieCreated));
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
    public async Task<Results<Ok<ApiResponse<MovieResponse>>, NotFound<ProblemDetails>, Conflict<ProblemDetails>>> UpdateMovie([FromBody] MovieUpdateRequest request)
    {
        var movieUpdated = await _movieService.UpdateMovie(request);
        if (movieUpdated is null)
        {
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Not Found",
                Detail = $"Movie not found"
            });
        }

        return TypedResults.Ok(ApiResponse<MovieResponse>.Success(movieUpdated));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMovie(long id)
    {
         try
        {
            bool isMovieDeleted = await _movieService.DeleteMovie(id);
            return StatusCode(StatusCodes.Status204NoContent, new { isSuccess = isMovieDeleted });
        }
        catch(Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, e.Message);
        }
    }

    [HttpGet]
    public async Task<Results<Ok<ApiResponse<List<MovieResponse>>>, NotFound<ProblemDetails>>> Get()
    {
        var movies = await _movieService.GetMovies();
        if (movies is null)
        {
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Not Found",
                Detail = "There are no movies"
            });
        }

        return TypedResults.Ok(ApiResponse<List<MovieResponse>>.Success(movies.ToList()));
    }


    [HttpGet]
    [Route("{id}")]
    public async Task<Results<Ok<ApiResponse<MovieResponse>>, NotFound<ProblemDetails>, Conflict<ProblemDetails>>> GetMovie(long id)
    {
        try
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

            return TypedResults.Ok(ApiResponse<MovieResponse>.Success(movie));
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


}
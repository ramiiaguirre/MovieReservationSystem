using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    [HttpPost]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateMovie([FromBody] MovieDTO request)
    {
            try
        {
            MovieDTO userCreated = await _movieService.CreateMovie(request);
            
            if (!string.IsNullOrEmpty(userCreated.Name))
                return StatusCode(StatusCodes.Status201Created, new { isSuccess = true });
            else 
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }
        catch(Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, e.Message);
        }
    }

}
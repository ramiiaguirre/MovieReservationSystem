using MovieReservation.Logic.UseCases;
using MovieReservation.Model.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Build.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MovieReservation.API.View.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    readonly IMovieCrudUseCases _movieCrudUseCases;
    public MoviesController(IMovieCrudUseCases movieCrudUseCases)
    {
        _movieCrudUseCases = movieCrudUseCases;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AddMovie(MovieDTO request)
    {
        var movie = await _movieCrudUseCases.ExecuteAdd(new Movie
        {
            Title = request.Title,
            Description = request.Description
        });

        if (movie.Id != 0)
        {
            return Ok(movie);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateMovie(MovieDTO request)
    {
        try
        {
            var movie = await _movieCrudUseCases.ExecuteUpdate(new Movie
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description
            });
            return Ok(movie);
        }
        catch
        {
            return BadRequest();
        }

    }

    [HttpGet]
    [Authorize(Roles = "Admin, Customer")]
    [Route("{id}")]
    public async Task<ActionResult> GetMovie(long id)
    {
        var movie = await _movieCrudUseCases.ExecuteGet(id);
        if (movie is null)
            return NotFound();
        else    
            return Ok(movie);
    }


    [HttpGet]
    [Authorize(Roles = "Admin, Customer")]
    public async Task<ActionResult> GetMovies()
    {
        var movies = await _movieCrudUseCases.ExecuteGet();
        if (movies is null)
            return NotFound();
        else    
            return Ok(movies);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteMovie(DeleteDTO request)
    {
        var delete = await _movieCrudUseCases.ExecuteDelete(request.Id);
        if(delete)
            return NoContent();
        else
            return BadRequest();
    }

}
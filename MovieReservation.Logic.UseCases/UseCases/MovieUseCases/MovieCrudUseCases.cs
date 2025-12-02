using System.Linq.Expressions;
using MovieReservation.Model.Domain;

namespace MovieReservation.Logic.UseCases;

public class MovieCrudUseCases : IMovieCrudUseCases
{
    private readonly IMovieUnitOfWork _movieUnitOfWork;
    public MovieCrudUseCases(IMovieUnitOfWork movieUnitOfWork)
    {
        _movieUnitOfWork = movieUnitOfWork;
    }

    public async Task<Movie> ExecuteAdd(Movie movie)
    {
        var movieCreated = await _movieUnitOfWork.Movies.Add(movie);
        await _movieUnitOfWork.Save();
        return movieCreated;
    }

    public async Task<bool> ExecuteDelete(long id)
    {
        var delete = await _movieUnitOfWork.Movies.Delete(id);
        if (delete)
        {
            await _movieUnitOfWork.Save();
        }
        return delete;
    }

    public async Task<Movie?> ExecuteGet(long id)
    {
        var movie = await _movieUnitOfWork.Movies.Get(id);
        return movie;
    }

    public async Task<IEnumerable<Movie>?> ExecuteGet()
    {
        var movies = await _movieUnitOfWork.Movies.Get();
        return null;
    }

    public async Task<Movie> ExecuteUpdate(Movie movie)
    {
       var movieUpdated = await _movieUnitOfWork.Movies.Update(movie);
       await _movieUnitOfWork.Save();
       return movieUpdated;
    }
}
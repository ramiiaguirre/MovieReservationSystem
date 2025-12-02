using MovieReservation.Model.Domain;

namespace MovieReservation.Logic.UseCases;

public interface IMovieCrudUseCases
{
    public Task<Movie> ExecuteAdd(Movie movie);
    public Task<Movie?> ExecuteGet(long id);

    public Task<IEnumerable<Movie>?> ExecuteGet();

    public Task<Movie> ExecuteUpdate(Movie movie);

    public Task<bool> ExecuteDelete(long id);
}
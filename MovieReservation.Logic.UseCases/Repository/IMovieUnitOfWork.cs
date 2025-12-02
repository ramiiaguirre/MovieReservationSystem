using MovieReservation.Model.Domain;

namespace MovieReservation.Logic.UseCases;

public interface IMovieUnitOfWork
{
    public IRepository<Movie> Movies { get; }
    public Task Save();
}
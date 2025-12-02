using MovieReservation.Logic.UseCases;
using MovieReservation.Model.Domain;

namespace MovieReservation.DataAccess.Infrastructure;
public class MovieUnitOfWork : IMovieUnitOfWork
{

    MovieReservationContext _context;
    IRepository<Movie>? _movies;

    public MovieUnitOfWork(MovieReservationContext context)
    {
        _context = context;
    }

    public IRepository<Movie> Movies  
        => _movies == null ? _movies = new RepositoryEF<Movie>(_context) : _movies;      


    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}
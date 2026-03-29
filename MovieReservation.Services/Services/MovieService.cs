using MovieReservation.Domain;

namespace MovieReservation.Services;

public class MovieService : IMovieService
{
    private readonly IRepository<Movie> _repository;
    public MovieService(IRepository<Movie> repository)
    {
        _repository = repository;
    }

    public async Task<MovieDTO> CreateMovie(MovieDTO request)
    {
        var movie = _repository.Get(request.Name);

        if (movie.Result is not null)
            throw new Exception($"Movie called {request.Name} already exist.");

        var movieCreated = await _repository.Add(new Movie()
        {
            Name = request.Name,
            Description = request.Description
        });

        await _repository.Save();

        return new MovieDTO()
        {
            Name = movieCreated.Name
        };
    }

    public Task<bool> DeleteMovie(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MovieDTO>> GetMovie(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MovieDTO>> GetMovies()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MovieDTO>> GetShowTimesOfMovie(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MovieDTO>> GetShowTimesOfMovie(long id, DateTime dateTime)
    {
        throw new NotImplementedException();
    }

    public Task<MovieDTO> UpdateMovie(MovieDTO request)
    {
        throw new NotImplementedException();
    }
}

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

    public async Task<bool> DeleteMovie(long id)
    {
        await _repository.Delete(id);
        return true;
    }

    public async Task<MovieDTO?> GetMovie(long id)
    {
        var movie = await _repository.Get(id);
        if (movie is null)
            return null;

        return new MovieDTO()
        {
            Id = movie.Id,
            Name = movie.Name,
            Description = movie.Description
        };
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

    public async Task<MovieDTO?> UpdateMovie(MovieDTO request)
    {
        var movie = await _repository.Get(request.Id);

        if (movie is null)
            return null;

        movie.Id = request.Id;
        movie.Name = request.Name;
        movie.Description = request.Description;
        
        movie = await _repository.Update(movie);

        return new MovieDTO()
        {
            Id = movie.Id,
            Name = movie.Name,
            Description = movie.Description
        };
    }
}

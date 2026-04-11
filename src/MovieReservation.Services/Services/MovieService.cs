using MovieReservation.Domain;

namespace MovieReservation.Services;

public class MovieService : IMovieService
{
    private readonly IRepository<Movie> _repository;
    public MovieService(IRepository<Movie> repository)
    {
        _repository = repository;
    }

    public async Task<MovieResponse> CreateMovie(MovieCreateRequest request)
    {
        var movie = _repository.Get(request.Name);

        if (movie.Result is not null)
            throw new Exception($"Movie called {request.Name} already exist.");

        var movieCreated = await _repository.Add(new Movie(request.Name, request.Description));

        await _repository.Save();

        return MovieResponse.FromMovie(movieCreated);
    }

    public async Task<bool> DeleteMovie(long id)
    {
        await _repository.Delete(id);
        return true;
    }

    public async Task<MovieResponse?> GetMovie(long id)
    {
        var movie = await _repository.Get(id);
        if (movie is null)
            return null;

        return MovieResponse.FromMovie(movie);
    }

    public async Task<IEnumerable<MovieResponse>> GetMovies()
    {
        var movies = await _repository.GetAll();
        return movies.Select((m) => MovieResponse.FromMovie(m));
    }

    public Task<IEnumerable<MovieResponse>> GetShowTimesOfMovie(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MovieResponse>> GetShowTimesOfMovie(long id, DateTime dateTime)
    {
        throw new NotImplementedException();
    }

    public async Task<MovieResponse?> UpdateMovie(MovieUpdateRequest request)
    {
        var movieId = request.Id ?? 0;
        var movie = await _repository.Get(movieId);

        if (movie is null)
            return null;

        movie.Id = movieId;

        if (request.Name is not null)
            movie.SetName(request.Name);
        
        movie.SetDescription(request.Description);
        
        movie = await _repository.Update(movie);

         await _repository.Save();

        return MovieResponse.FromMovie(movie);
    }
}

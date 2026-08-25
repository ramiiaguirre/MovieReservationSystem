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
        // var movie = _repository.Get(request.Name);
        var movie = _repository.FindOne(m => m.Name == request.Name);

        if (movie.Result is not null)
            throw new Exception($"Movie called {request.Name} already exist.");

        var movieCreated = await _repository.Add(new Movie(request.Name, request.Genre, request.Description));

        await _repository.Save();

        return MovieResponse.FromMovie(movieCreated);
    }

    public async Task<bool> DeleteMovie(long id)
    {
        var movie = await _repository.Get(id);
        if (movie is null) {
            return false;
        }
        await _repository.Delete(id);
        await _repository.Save();
        return true;
    }

    public async Task<MovieResponse?> GetMovie(long id)
    {
        var movie = await _repository.Get(id);
        if (movie is null)
            return null;

        return MovieResponse.FromMovie(movie);
    }

    public async Task<IEnumerable<MovieResponse>> GetMovies(Genre? genre = null)
    {
        var movies = genre is null
            ? await _repository.GetAll()
            : await _repository.Find(m => m.Genre == genre);

        return movies.Select((m) => MovieResponse.FromMovie(m));
    }

    public async Task<MovieResponse?> UpdateMovie(MovieUpdateRequest request)
    {
        var movie = await _repository.Get(request.Id);

        if (movie is null)
            return null;

        movie.Id = request.Id;

        if (request.Name is not null)
            movie.SetName(request.Name);

        if (request.Genre is not null)
            movie.SetGenre(request.Genre.Value);

        movie.SetDescription(request.Description);

        movie = await _repository.Update(movie);

         await _repository.Save();

        return MovieResponse.FromMovie(movie);
    }
}

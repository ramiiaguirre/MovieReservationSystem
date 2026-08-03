using MovieReservation.Domain;

namespace MovieReservation.Services;

public interface IMovieService
{
    public Task<MovieResponse> CreateMovie(MovieCreateRequest request);
    public Task<MovieResponse?> UpdateMovie(MovieUpdateRequest request);
    public Task<bool> DeleteMovie(long id);
    public Task<IEnumerable<MovieResponse>> GetMovies(Genre? genre = null);
    public Task<MovieResponse?> GetMovie(long id);
}

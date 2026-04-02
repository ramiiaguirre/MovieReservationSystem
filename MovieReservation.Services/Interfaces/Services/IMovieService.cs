namespace MovieReservation.Services;

public interface IMovieService
{
    public Task<MovieDTO> CreateMovie(MovieDTO request);
    public Task<MovieDTO?> UpdateMovie(MovieDTO request);
    public Task<bool> DeleteMovie(long id);
    public Task<IEnumerable<MovieDTO>> GetMovies();
    public Task<MovieDTO?> GetMovie(long id);
    public Task<IEnumerable<MovieDTO>> GetShowTimesOfMovie(long id);
    public Task<IEnumerable<MovieDTO>> GetShowTimesOfMovie(long id, DateTime dateTime);
}

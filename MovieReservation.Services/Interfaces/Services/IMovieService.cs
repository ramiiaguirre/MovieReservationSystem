namespace MovieReservation.Services;

public interface IMovieService
{
	// - [POST]	/movies 									#Create Movie - ADMIN
	// - [PUT] 	/movies										#Update Movie - ADMIN
	// - [DELETE]	/movies/{id_movie}  						#Delete Movie - ADMIN
	
	// - [GET]		/movies?{filters} 							#List filtering movies. For example: genre
	// - [GET]		/movies/{id_movie}							#See movie detail	
	// - [GET]		/movies/{id_movie}/showtimes				#See showtimes of the movie
	// - [GET]		/movies/{id_movie}/showtimes?date={date}	#See showtimes of the movie for a specific date

    public Task<MovieDTO> CreateMovie(MovieDTO request);
    public Task<MovieDTO> UpdateMovie(MovieDTO request);
    public Task<bool> DeleteMovie(long id);



    public Task<IEnumerable<MovieDTO>> GetMovies();
    public Task<IEnumerable<MovieDTO>> GetMovie(long id);
    public Task<IEnumerable<MovieDTO>> GetShowTimesOfMovie(long id);
    public Task<IEnumerable<MovieDTO>> GetShowTimesOfMovie(long id, DateTime dateTime);

    
    
}

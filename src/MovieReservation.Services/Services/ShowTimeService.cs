using MovieReservation.Domain;

namespace MovieReservation.Services;

public class ShowTimeService : IShowTimeService
{
    private readonly IRepository<ShowTime> _showTimeRepository;
    private readonly IRepository<Movie> _movieRepository;
    private readonly IRepository<TheaterRoom> _theaterRoomRepository;
    private readonly IRepository<Seat> _seatRepository;
    private readonly IRepository<ReservationSeat> _reservationSeatRepository;

    public ShowTimeService(
        IRepository<ShowTime> showTimeRepository,
        IRepository<Movie> movieRepository,
        IRepository<TheaterRoom> theaterRoomRepository,
        IRepository<Seat> seatRepository,
        IRepository<ReservationSeat> reservationSeatRepository)
    {
        _showTimeRepository = showTimeRepository;
        _movieRepository = movieRepository;
        _theaterRoomRepository = theaterRoomRepository;
        _seatRepository = seatRepository;
        _reservationSeatRepository = reservationSeatRepository;
    }

    public async Task<ShowTimeResponse> Create(ShowTimeCreateRequest request)
    {
        var movie = await _movieRepository.Get(request.MovieId)
            ?? throw new Exception($"Movie {request.MovieId} not found.");

        var room = await _theaterRoomRepository.Get(request.TheaterRoomId)
            ?? throw new Exception($"TheaterRoom {request.TheaterRoomId} not found.");

        var overlapping = await _showTimeRepository.Find(
            st => st.TheaterRoomId == request.TheaterRoomId && st.ShowDateTime == request.ShowDateTime);

        if (overlapping.Any())
            throw new Exception("There is already a showtime scheduled for that room at that date/time.");

        var showTime = await _showTimeRepository.Add(
            new ShowTime(request.MovieId, request.TheaterRoomId, request.ShowDateTime, request.Duration, request.Price));

        await _showTimeRepository.Save();

        return BuildResponse(showTime, movie, room);
    }

    public async Task<ShowTimeResponse?> Update(ShowTimeUpdateRequest request)
    {
        var showTime = await _showTimeRepository.Get(request.Id);
        if (showTime is null)
            return null;

        if (request.ShowDateTime is not null)
            showTime.ShowDateTime = request.ShowDateTime.Value;

        if (request.Duration is not null)
            showTime.Duration = request.Duration.Value;

        if (request.Price is not null)
            showTime.Price = request.Price.Value;

        if (request.IsActive is not null)
            showTime.IsActive = request.IsActive.Value;

        showTime = await _showTimeRepository.Update(showTime);
        await _showTimeRepository.Save();

        var movie = await _movieRepository.Get(showTime.MovieId);
        var room = await _theaterRoomRepository.Get(showTime.TheaterRoomId);

        return BuildResponse(showTime, movie, room);
    }

    public async Task<bool> Delete(long id)
    {
        var showTime = await _showTimeRepository.Get(id);
        if (showTime is null)
            return false;

        await _showTimeRepository.Delete(id);
        await _showTimeRepository.Save();
        return true;
    }

    public async Task<ShowTimeResponse?> GetById(long id)
    {
        var showTimes = await _showTimeRepository.Find(st => st.Id == id, st => st.Movie, st => st.TheaterRoom);
        var showTime = showTimes.FirstOrDefault();
        return showTime is null ? null : ShowTimeResponse.FromShowTime(showTime);
    }

    public async Task<IEnumerable<ShowTimeResponse>> GetAll(long? movieId = null, DateTime? date = null)
    {
        IEnumerable<ShowTime> showTimes;

        if (movieId is not null && date is not null)
            showTimes = await _showTimeRepository.Find(
                st => st.MovieId == movieId.Value && st.ShowDateTime.Date == date.Value.Date,
                st => st.Movie, st => st.TheaterRoom);
        else if (movieId is not null)
            showTimes = await _showTimeRepository.Find(
                st => st.MovieId == movieId.Value, st => st.Movie, st => st.TheaterRoom);
        else if (date is not null)
            showTimes = await _showTimeRepository.Find(
                st => st.ShowDateTime.Date == date.Value.Date, st => st.Movie, st => st.TheaterRoom);
        else
            showTimes = await _showTimeRepository.Find(st => true, st => st.Movie, st => st.TheaterRoom);

        return showTimes.Select(ShowTimeResponse.FromShowTime);
    }

    public async Task<IEnumerable<SeatAvailabilityResponse>?> GetSeatsAvailability(long showtimeId)
    {
        var showTime = await _showTimeRepository.Get(showtimeId);
        if (showTime is null)
            return null;

        var seats = await _seatRepository.Find(s => s.TheaterRoomId == showTime.TheaterRoomId && s.IsActive);
        var reservedSeats = await _reservationSeatRepository.Find(rs => rs.ShowTimeId == showtimeId);
        var reservedSeatIds = reservedSeats.Select(rs => rs.SeatId).ToHashSet();

        return seats.Select(s => new SeatAvailabilityResponse(
            s.Id, s.RowLetter, s.SeatNumber, s.SeatType, !reservedSeatIds.Contains(s.Id)));
    }

    private static ShowTimeResponse BuildResponse(ShowTime showTime, Movie? movie, TheaterRoom? room) =>
        new(
            showTime.Id,
            showTime.MovieId,
            movie?.Name ?? string.Empty,
            showTime.TheaterRoomId,
            room?.Name ?? string.Empty,
            showTime.ShowDateTime,
            showTime.Duration,
            showTime.Price,
            showTime.IsActive
        );
}

namespace MovieReservation.Services;

public interface IShowTimeService
{
    Task<ShowTimeResponse> Create(ShowTimeCreateRequest request);
    Task<ShowTimeResponse?> Update(ShowTimeUpdateRequest request);
    Task<bool> Delete(long id);
    Task<ShowTimeResponse?> GetById(long id);
    Task<IEnumerable<ShowTimeResponse>> GetAll(long? movieId = null, DateTime? date = null);
    Task<IEnumerable<SeatAvailabilityResponse>?> GetSeatsAvailability(long showtimeId);
}

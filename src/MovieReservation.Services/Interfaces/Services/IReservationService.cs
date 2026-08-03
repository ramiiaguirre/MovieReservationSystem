namespace MovieReservation.Services;

public interface IReservationService
{
    Task<ReservationResponse> Create(long userId, ReservationCreateRequest request);
    Task<ReservationResponse?> GetById(long id, long currentUserId, bool isAdmin);
    Task<IEnumerable<ReservationResponse>> GetByUser(long userId);
    Task<IEnumerable<ReservationResponse>> List(long currentUserId, bool isAdmin, long? userId = null);
    Task<ReservationResponse?> Cancel(long id, long currentUserId, bool isAdmin);
}

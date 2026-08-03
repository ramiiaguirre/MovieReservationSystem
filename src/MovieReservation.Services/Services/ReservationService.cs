using Microsoft.EntityFrameworkCore;
using MovieReservation.Domain;

namespace MovieReservation.Services;

public class ReservationService : IReservationService
{
    private readonly IRepository<Reservation> _reservationRepository;
    private readonly IRepository<ReservationSeat> _reservationSeatRepository;
    private readonly IRepository<ShowTime> _showTimeRepository;
    private readonly IRepository<Seat> _seatRepository;

    public ReservationService(
        IRepository<Reservation> reservationRepository,
        IRepository<ReservationSeat> reservationSeatRepository,
        IRepository<ShowTime> showTimeRepository,
        IRepository<Seat> seatRepository)
    {
        _reservationRepository = reservationRepository;
        _reservationSeatRepository = reservationSeatRepository;
        _showTimeRepository = showTimeRepository;
        _seatRepository = seatRepository;
    }

    public async Task<ReservationResponse> Create(long userId, ReservationCreateRequest request)
    {
        var showTime = await _showTimeRepository.Get(request.ShowTimeId)
            ?? throw new Exception($"Showtime {request.ShowTimeId} not found.");

        if (!showTime.IsActive)
            throw new Exception("This showtime is not active.");

        if (showTime.ShowDateTime <= DateTime.UtcNow)
            throw new Exception("Cannot reserve seats for a showtime that has already started.");

        var seatIds = request.SeatIds.Distinct().ToList();

        var seats = (await _seatRepository.Find(s => seatIds.Contains(s.Id))).ToList();
        if (seats.Count != seatIds.Count)
            throw new Exception("One or more seats do not exist.");

        if (seats.Any(s => s.TheaterRoomId != showTime.TheaterRoomId))
            throw new Exception("One or more seats do not belong to the showtime's room.");

        if (seats.Any(s => !s.IsActive))
            throw new Exception("One or more seats are not available.");

        var alreadyReserved = await _reservationSeatRepository.Find(
            rs => rs.ShowTimeId == request.ShowTimeId && seatIds.Contains(rs.SeatId));

        if (alreadyReserved.Any())
            throw new Exception("One or more seats are already reserved for this showtime.");

        var totalAmount = showTime.Price * seatIds.Count;
        var reservation = new Reservation(userId, request.ShowTimeId, totalAmount);

        foreach (var seatId in seatIds)
            reservation.Seats.Add(new ReservationSeat(0, seatId, request.ShowTimeId, showTime.Price));

        reservation.Confirm();

        reservation = await _reservationRepository.Add(reservation);

        try
        {
            await _reservationRepository.Save();
        }
        catch (DbUpdateException)
        {
            throw new Exception("One or more seats were just reserved by someone else. Please try again.");
        }

        return BuildResponse(reservation, seats);
    }

    public async Task<ReservationResponse?> GetById(long id, long currentUserId, bool isAdmin)
    {
        var reservation = (await _reservationRepository.Find(r => r.Id == id, r => r.Seats)).FirstOrDefault();
        if (reservation is null)
            return null;

        if (!isAdmin && reservation.UserId != currentUserId)
            throw new UnauthorizedAccessException("You cannot view a reservation that is not yours.");

        var seats = await GetSeatsFor(reservation.Seats);
        return BuildResponse(reservation, seats);
    }

    public async Task<IEnumerable<ReservationResponse>> GetByUser(long userId)
    {
        var reservations = await _reservationRepository.Find(r => r.UserId == userId, r => r.Seats);
        return await BuildResponses(reservations);
    }

    public async Task<IEnumerable<ReservationResponse>> List(long currentUserId, bool isAdmin, long? userId = null)
    {
        var targetUserId = isAdmin ? userId : currentUserId;

        var reservations = targetUserId is not null
            ? await _reservationRepository.Find(r => r.UserId == targetUserId.Value, r => r.Seats)
            : await _reservationRepository.Find(r => true, r => r.Seats);

        return await BuildResponses(reservations);
    }

    public async Task<ReservationResponse?> Cancel(long id, long currentUserId, bool isAdmin)
    {
        var reservation = (await _reservationRepository.Find(r => r.Id == id, r => r.Seats)).FirstOrDefault();
        if (reservation is null)
            return null;

        if (!isAdmin && reservation.UserId != currentUserId)
            throw new UnauthorizedAccessException("You cannot cancel a reservation that is not yours.");

        if (reservation.Status == "cancelled")
            throw new Exception("This reservation is already cancelled.");

        foreach (var reservationSeat in reservation.Seats.ToList())
            await _reservationSeatRepository.Delete(reservationSeat.Id);

        reservation.Seats.Clear();
        reservation.Cancel();

        reservation = await _reservationRepository.Update(reservation);
        await _reservationRepository.Save();

        return BuildResponse(reservation, Enumerable.Empty<Seat>());
    }

    private async Task<IEnumerable<Seat>> GetSeatsFor(IEnumerable<ReservationSeat> reservationSeats)
    {
        var seatIds = reservationSeats.Select(rs => rs.SeatId).Distinct().ToList();
        return seatIds.Count > 0
            ? await _seatRepository.Find(s => seatIds.Contains(s.Id))
            : Enumerable.Empty<Seat>();
    }

    private async Task<IEnumerable<ReservationResponse>> BuildResponses(IEnumerable<Reservation> reservations)
    {
        var seatIds = reservations.SelectMany(r => r.Seats.Select(rs => rs.SeatId)).Distinct().ToList();
        var seats = seatIds.Count > 0
            ? await _seatRepository.Find(s => seatIds.Contains(s.Id))
            : Enumerable.Empty<Seat>();

        return reservations.Select(r => BuildResponse(r, seats));
    }

    private static ReservationResponse BuildResponse(Reservation reservation, IEnumerable<Seat> seats)
    {
        var seatById = seats.ToDictionary(s => s.Id);

        var seatResponses = reservation.Seats.Select(rs =>
        {
            seatById.TryGetValue(rs.SeatId, out var seat);
            return new ReservationSeatResponse(rs.SeatId, seat?.RowLetter ?? string.Empty, seat?.SeatNumber ?? 0, rs.Price);
        }).ToList();

        return new ReservationResponse(
            reservation.Id,
            reservation.ReservationCode,
            reservation.UserId,
            reservation.ShowTimeId,
            reservation.TotalAmount,
            reservation.Status,
            reservation.CreatedAt,
            reservation.PaymentDate,
            seatResponses
        );
    }
}

using MovieReservation.Model.Domain;

namespace MovieReservation.Logic.UseCases;
public interface IRoleUseCases
{
    Task<IEnumerable<User>> ExecuteAddRole();
}
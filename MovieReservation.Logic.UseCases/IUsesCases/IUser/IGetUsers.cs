using MovieReservation.Model.Domain;

public interface IGetUsers
{
    Task<IEnumerable<User>> Execute();
}

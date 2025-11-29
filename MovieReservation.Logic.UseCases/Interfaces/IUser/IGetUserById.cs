using MovieReservation.Model.Domain;

public interface IGetUserById
{
    Task<User?> Execute(long id, bool modelComplete = false);
}
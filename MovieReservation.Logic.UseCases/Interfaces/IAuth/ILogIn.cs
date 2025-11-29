using MovieReservation.Model.Domain;

public interface ILogIn
{
    Task<User?> Execute(string name, string passwordHash);
}
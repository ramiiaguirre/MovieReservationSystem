using MovieReservation.Logic.UseCases;
using MovieReservation.Model.Domain;

public class LogIn : ILogIn
{
    private readonly IUserUnitOfWork _userUnitOfWork;
    public LogIn(IUserUnitOfWork userUnitOfWork)
    {
        _userUnitOfWork = userUnitOfWork;
    }

    public async Task<User?> Execute(string name, string passwordHash)
    {
        var loggedInUser = await _userUnitOfWork.Users.Get(
            u => u.Name == name && u.Password == passwordHash, 
            u => u.Roles!
        );
        return loggedInUser;
    }
    
}
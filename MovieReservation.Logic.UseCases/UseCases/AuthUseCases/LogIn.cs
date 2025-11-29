using MovieReservation.Logic.Repository;
using MovieReservation.Model.Domain;

public class LogIn : ILogIn
{
    private readonly IRepository<User> _repository = default!;
    public LogIn(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<User?> Execute(string name, string passwordHash)
    {
        var loggedInUser = await _repository.Get(
            u => u.Name == name && u.Password == passwordHash, 
            u => u.Roles!
        );
        return loggedInUser;
    }
    
}
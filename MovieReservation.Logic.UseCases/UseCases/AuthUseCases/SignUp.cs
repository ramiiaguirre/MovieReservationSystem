using MovieReservation.Logic.UseCases;
using MovieReservation.Model.Domain;

public class SignUp : ISignUp
{
    private readonly IUserUnitOfWork _userUnitOfWork = default!;
    public SignUp(IUserUnitOfWork userUnitOfWork)
    {
        _userUnitOfWork = userUnitOfWork;
    }

    public async Task<User> Execute(User user)
    {
        var userCreated = await _userUnitOfWork.Users.Add(user);
        await _userUnitOfWork.Save();
        return userCreated;
    }
    
}
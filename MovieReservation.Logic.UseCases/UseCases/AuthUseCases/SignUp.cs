using MovieReservation.Logic.Repository;
using MovieReservation.Model.Domain;

public class SignUp : ISignUp
{
    private readonly IRepository<User> _repository = default!;
    public SignUp(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<User> Execute(User user)
    {
        var userCreated = await _repository.Add(user);
        await _repository.Save();
        return userCreated;
    }
    
}
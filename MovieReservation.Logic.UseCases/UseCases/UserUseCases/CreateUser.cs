using MovieReservation.Logic.Repository;
using MovieReservation.Model.Domain;

public class CreateUser : ICreateUser
{
    private readonly IRepository<User> _repository = default!;
    public CreateUser(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task Execute(User user)
    {
        await _repository.Add(user);
    }
}
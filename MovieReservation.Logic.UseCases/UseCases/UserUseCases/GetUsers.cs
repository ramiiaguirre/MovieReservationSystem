using MovieReservation.Logic.Repository;
using MovieReservation.Model.Domain;

public class GetUsers : IGetUsers
{
    private readonly IRepository<User> _repository = default!;
    public GetUsers(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<User>> Execute()
    {
        var users = await _repository.Get();
        return users;
    }
}
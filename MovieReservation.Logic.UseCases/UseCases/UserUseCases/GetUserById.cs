using MovieReservation.Logic.Repository;
using MovieReservation.Model.Domain;

public class GetUserById : IGetUserById
{
    private readonly IRepository<User> _repository = default!;
    public GetUserById(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<User?> Execute(long id, bool modelComplete = false)
    {
        if (modelComplete)
            return await _repository.Get(id, u => u.Roles!);

        return await _repository.Get(id);
    }
}
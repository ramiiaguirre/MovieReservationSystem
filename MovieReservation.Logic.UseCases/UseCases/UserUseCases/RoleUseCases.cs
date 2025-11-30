using MovieReservation.Model.Domain;

namespace MovieReservation.Logic.UseCases;
public class RoleUseCases : IRoleUseCases
{
    readonly IUserUnitOfWork _userUnitOfWork;
    
    public RoleUseCases(IUserUnitOfWork userUnitOfWork)
    {
        _userUnitOfWork = userUnitOfWork;
    }

    public async Task<IEnumerable<User>> ExecuteAddRole()
    {
        var users = await _userUnitOfWork.Users.Get();
        return users;
    }

}
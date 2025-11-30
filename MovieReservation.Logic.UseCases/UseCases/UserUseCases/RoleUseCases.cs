using MovieReservation.Model.Domain;

namespace MovieReservation.Logic.UseCases;
public class RoleUseCases : IRoleUseCases
{
    readonly IUserUnitOfWork _userUnitOfWork;
    
    public RoleUseCases(IUserUnitOfWork userUnitOfWork)
    {
        _userUnitOfWork = userUnitOfWork;
    }

    public async Task<Role> ExecuteAddRole(Role role)
    {
        var roleCreated = await _userUnitOfWork.Roles.Add(role);
        await _userUnitOfWork.Save();
        return roleCreated;
    }

    public async Task ExecuteAddRoleToUser(long idUser, long idRole)
    {
        // Obtener el usuario y el role
        var user = await _userUnitOfWork.Users.Get(idUser, u => u.Roles);
        if (user is null)
            return;

        var role = await _userUnitOfWork.Roles.Get(idRole);
        if (role is null)
            return;
        
        user!.Roles!.Add(role);
        await _userUnitOfWork.Save();
    }
}
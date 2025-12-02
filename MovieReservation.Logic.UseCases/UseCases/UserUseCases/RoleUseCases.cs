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

    public async Task ExecuteAddRoleToUser(long idUser, long idRole, long idUserWhoAssigns)
    {
        // Verificar que no exista la relación
        var exists = await _userUnitOfWork.UserRoles
            .Get(ur => ur.UserRole_User_FK == idUser && ur.UserRole_Role_FK == idRole);
        
        if (exists is not null)
            throw new Exception("Relationship already exists");
            
        var userRole = new UserRole
        {
            UserRole_User_FK = idUser,
            UserRole_Role_FK = idRole,
            AssignedAt = DateTime.Now.Date,
            AssignedBy = idUserWhoAssigns
        };
        
        await _userUnitOfWork.UserRoles.Add(userRole);
        await _userUnitOfWork.Save();
    }
}
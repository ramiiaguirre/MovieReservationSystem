using MovieReservation.Model.Domain;

namespace MovieReservation.Logic.UseCases;
public interface IRoleUseCases
{
    Task<Role> ExecuteAddRole(Role role);

    Task ExecuteAddRoleToUser(long idUser, long idRole);
}
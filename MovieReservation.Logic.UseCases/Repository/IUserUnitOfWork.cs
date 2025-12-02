using MovieReservation.Model.Domain;

namespace MovieReservation.Logic.UseCases;
public interface IUserUnitOfWork
{
    public IRepository<User> Users { get; }
    public IRepository<UserRole> UserRoles { get; }
    public IRepository<Role> Roles { get; }
    public IRepository<Permission> Permissions { get; }

    public Task Save();
}
using MovieReservation.Logic.UseCases;
using MovieReservation.Model.Domain;

namespace MovieReservation.DataAccess.Infrastructure;
public class UserUnitOfWork : IUserUnitOfWork
{

    MovieReservationContext _context;
    IRepository<User>? _users;
    IRepository<Role>? _roles;
    IRepository<Permission>? _permissions;

    public UserUnitOfWork(MovieReservationContext context)
    {
        _context = context;
    }

    public IRepository<User> Users  
        => _users == null ? _users = new RepositoryEF<User>(_context) : _users;      

    public IRepository<Role> Roles
        => _roles == null ? _roles = new RepositoryEF<Role>(_context) : _roles;

    public IRepository<Permission> Permissions 
        => _permissions == null ? _permissions = new RepositoryEF<Permission>(_context) : _permissions;

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}
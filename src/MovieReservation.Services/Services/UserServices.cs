using MovieReservation.API;
using MovieReservation.Domain;

namespace MovieReservation.Services;

public class UserService
{
    private readonly IRepository<User> _userRepository = default!;
    private readonly IRepository<Rol> _rolRepository = default!;

    public UserService(IRepository<User> userRepo, IRepository<Rol> rolRepo)
    {
        _userRepository = userRepo;
        _rolRepository = rolRepo;
    }


    public async Task<RolDTO> CreateRol(RolDTO request)
    {

        var rol = _rolRepository.Get(request.Name);

        if (rol.Result is not null)
            throw new Exception($"Rol called {request.Name} already exist.");

        var rolCreated = await _rolRepository.Add(new Rol()
        {
            Name = request.Name,
            Description = request.Description
        });

        await _rolRepository.Save();

        return request;
    }

    public async Task<List<RolDTO>> GetRoles()
    {
        var roles = await _rolRepository.GetAll();
        return roles.Select(e => new RolDTO()
        {
            Name = e.Name,
            Description = e.Description
        }).ToList();
    }

    public async Task<bool> AddRolToUser(RolToUserRequest request)
    {
        var user = await _userRepository.Get(request.UserId, u => u.Roles)
            ?? throw new KeyNotFoundException("User not found");  

        var rol = await _rolRepository.Get(request.RolName)
            ??  throw new KeyNotFoundException("Rol not found");  

        if (user.Roles.Any(r => r.Id == rol.Id))
            return false;
        
        user.Roles.Add(rol);
        // await _userRepository.Update(user);
        await _userRepository.Save();
        return true;
            
    }

}
